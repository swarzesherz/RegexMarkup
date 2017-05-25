using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using Microsoft.Office.Interop.Word;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using RegexMarkup.Forms;

namespace RegexMarkup
{
    public sealed class Ecuation2Markup
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static Ecuation2Markup instance = null;
        static readonly object padlock = new object();
        public static Ecuation2Markup Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Ecuation2Markup();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private static Word.Document ActiveDocument = null;
        private String mml2markupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mml2markup.xsl");
        private String omml2mmlPath = Path.Combine(Globals.ThisAddIn.Application.Path, "OMML2MML.XSL");
        String jeuclidPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "JEuclid");
        private Boolean jeuclidExist = false;
        private Waiting waitForm = null;
        private Regex texPattern = new Regex(@"\\\[(.+?)\\\]|(?<![$])\$\$(.+?)\$\$(?!\$)|(?<![$])\$([^$]+)\$(?!\$)", RegexOptions.Singleline);

        public void initialize() {
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            jeuclidExist = File.Exists(Path.Combine(jeuclidPath, "bin/mml2xxx.bat"));
        }

        public void convertSelection2MML(bool showAlert=true)
        {
            Word.Selection docSeleccion = Globals.ThisAddIn.Application.Selection;
            int searchLenght = 255;
            String mml = "";
            List<Tuple<String, Range>> texEquations = new List<Tuple<String, Range>> { };
            Match matchResults = texPattern.Match(docSeleccion.Text);
            if (!matchResults.Success)
            {
                if(showAlert) MessageBox.Show("No se encontraron ecuaciones TeX");
                return;
            }
            Word.Range frng = docSeleccion.Range;
            Word.Find findObject = frng.Find;
            findObject.ClearFormatting();

            while (matchResults.Success)
            {
                int rngStart = 0;
                int rngEnd = 0;
                bool foundWord = false;
                mml = tex2mml(matchResults.Groups[1].Value + matchResults.Groups[2].Value + matchResults.Groups[3].Value);
                if (mml == "")
                {
                    matchResults = matchResults.NextMatch();
                    continue;
                }
                String searchText = matchResults.Value.Replace("^", "^^");

                int parts = searchText.Length / searchLenght;
                for (int i = 0; i <= parts; i++) {
                    int lenght = searchLenght;
                    if ((i + 1) * searchLenght > searchText.Length)
                        lenght = searchText.Length % searchLenght;
                    string search = searchText.Substring(i * searchLenght, lenght);
                    findObject.Text = search;
                    if (findObject.Execute())
                    {
                        if (i == 0) rngStart = frng.Start;
                        rngEnd = frng.End;
                        frng = ActiveDocument.Range(rngEnd, docSeleccion.Range.End);
                        findObject = frng.Find;
                        findObject.ClearFormatting();
                        foundWord = true;
                    }
                }

                if (!foundWord)
                {
                    rngStart = docSeleccion.Range.Start + matchResults.Index;
                    rngEnd = rngStart + matchResults.Length;
                }
                texEquations.Add(Tuple.Create(mml, ActiveDocument.Range(rngStart, rngEnd)));
                matchResults = matchResults.NextMatch();
            }

            for (int i = 0; i < texEquations.Count; i++)
            {
                Clipboard.Clear();
                Clipboard.SetText(texEquations[i].Item1, TextDataFormat.UnicodeText);
                texEquations[i].Item2.Font.ColorIndex = WdColorIndex.wdBlack;
                texEquations[i].Item2.Paste();
            }
        }
        
        public void convertSelection() {
            Word.Selection docSeleccion = Globals.ThisAddIn.Application.Selection;
            this.convertSelection2MML(false);

            if (docSeleccion.OMaths.Count <= 0)
            {
                MessageBox.Show("No se encontraron ecuaciones");
                return;
            }

            foreach (OMath equation in docSeleccion.OMaths)
            {
                this.convert(equation);
            }
        }

        public void convertAll() {

            int omaths = 0;

            foreach (Word.Footnote footnote in ActiveDocument.Footnotes){
                omaths += footnote.Range.OMaths.Count;
            }

            foreach (Word.Endnote endnote in ActiveDocument.Endnotes){
                omaths += endnote.Range.OMaths.Count;
            }

            omaths += ActiveDocument.OMaths.Count;

            if (omaths <= 0)
            {
                MessageBox.Show("No se encontraron ecuaciones");
                return;
            }
            DialogResult result;
            

            result = MessageBox.Show(String.Format("Se encontraron {0} ecuaciones.\n Continuar?", omaths), "", MessageBoxButtons.YesNo);

            if(result == DialogResult.No){
                return;
            }

            /* Ocultamos la aplicacion durante los procesos de reemplazo y coloreado para hacer mas rapida la aplicacion */
            Globals.ThisAddIn.Application.Visible = false;
            waitForm = Waiting.Instance;
            waitForm.Show();
            foreach (OMath equation in ActiveDocument.OMaths)
            {
                this.convert(equation);
            }

            foreach (Word.Footnote footnote in ActiveDocument.Footnotes)
            {
                foreach (OMath equation in footnote.Range.OMaths)
                {
                    this.convert(equation);
                }
            }

            foreach (Word.Endnote endnote in ActiveDocument.Endnotes)
            {
                foreach (OMath equation in endnote.Range.OMaths)
                {
                    this.convert(equation);
                }
            }
            /* Mostramos de nuevo la aplicacion */
            waitForm.Hide();
            Globals.ThisAddIn.Application.Visible = true;
        }

        public void revertAll(){

        }

        public void revert() {
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            Regex regexObj = null;
            Match matchResults = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            String pattern = @".*?(\[mml:math.+?\[/mml:math\]).*";
            regexObj = new Regex(pattern, options);

            List<Tuple<String, Range>> equations = new List<Tuple<String, Range>>{};

            Word.Range frng = ActiveDocument.Content;
            Word.Find findObject = frng.Find;
            findObject.ClearFormatting();
            findObject.Text = @"\[equation*\]*\[/mml:math\]*\[/equation\]";
            findObject.MatchWildcards = true;

            while (findObject.Execute())
            {
                matchResults = regexObj.Match(frng.Text);
                String mmlstr = matchResults.Groups[1].Value;
                mmlstr = mmlstr
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;")
                    .Replace('[', '<')
                    .Replace(']', '>')
                    .Replace("&#91;", "[")
                    .Replace("&#93;", "]");
                equations.Add(Tuple.Create(mmlstr, ActiveDocument.Range(frng.Start, frng.End)));
            }

            for (int i = 0; i < equations.Count; i++){
                Clipboard.Clear();
                Clipboard.SetText(equations[i].Item1, TextDataFormat.UnicodeText);
                equations[i].Item2.Font.ColorIndex = WdColorIndex.wdBlack;
                equations[i].Item2.Paste();
            }
        }

        public void convert(OMath equation) {
            this.convert(equation, true, true);
        }

        private String tex2mml(String tex) {
            String result = "";
            String command = "";
            tex = tex.Replace("\r", " ").Replace("\n", " ");
            if (tex.Substring(0, 1) == "-")
                tex = " -" + tex.Substring(1);
            command = String.Format(@"/C tex2mml ""{0}"" > tex.mml", Regex.Replace(tex, @"(\\*)" + "\"", @"$1$1\" + "\""));

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = ActiveDocument.Path;

            startInfo.RedirectStandardError = true;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            string error = process.StandardError.ReadToEnd();
            if (error != "")
                return result;
            result = File.ReadAllText(Path.Combine(ActiveDocument.Path, "tex.mml"));

            return result;
        }

        public void convert(OMath equation, Boolean mml_convert, Boolean img_convert) {
            object missing = Type.Missing;
            XmlDocument ommlxml = new XmlDocument();
            XmlDocument mmlxml = new XmlDocument();
            XslCompiledTransform xslt = new XslCompiledTransform();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            StringWriter writer = new StringWriter();
            RichTextBox markedRtb = new RichTextBox();
            String graphicTag = "";
            String mmlmathTag = "";
            String mmlMarkup = "";
            String equationTag = "[equation]{0}{1}[/equation]";
            String mmlPath = Path.Combine(Path.GetTempPath(), "equation.mml");
            String imgPath = Path.Combine(Path.GetTempPath(), "equation.png");
            int equationStart = 0;
            int imgStart = 0;
            Regex tagExp = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            Match matchResults = null;
            Dictionary<String, Color> colors = new Dictionary<String, Color>() { 
                {"equation", Color.FromArgb(127, 15, 126)},
                {"graphic", Color.FromArgb(56, 152, 103)},
                {"mmlmath", Color.FromArgb(24, 255, 254)},
                {"mml:", Color.LightGray}
            };
            
            /* Get XML from equation*/
            ommlxml.LoadXml(equation.Range.WordOpenXML);
            writerSettings.OmitXmlDeclaration = true;

            /* Transfrom omml ecuation to mml*/
            xslt.Load(omml2mmlPath);
            xslt.Transform(ommlxml, XmlWriter.Create(writer, writerSettings));
            mmlxml.LoadXml(writer.ToString());

            /* Transform mml to markup mml */
            writer = new StringWriter();
            xslt.Load(mml2markupPath);
            xslt.Transform(mmlxml, null, writer);
            mmlMarkup = writer.ToString().Replace("[mml:math]", "[mml:math xmlns:mml=\"http://www.w3.org/1998/Math/MathML\"]");
            //mmlMarkup = writer.ToString().Replace("[mml:math]", "[mml:math xmlns:mml=\"http://www.w3.org/1998/Math/MathML\" display=\"block\"]"); ;

            /*Add display mode*/
            //mmlxml.DocumentElement.Attributes.Append(mmlxml.CreateAttribute("display"));
            //mmlxml.DocumentElement.Attributes["display"].Value = "block";
            /* Get img fron range */
            equation.Range.CopyAsPicture();

            /* Set parent tags */
            if (mml_convert) 
                mmlmathTag = String.Format("[mmlmath]{0}[/mmlmath]", mmlMarkup);
            if (img_convert && jeuclidExist)
                graphicTag = String.Format("[graphic href=\"?{0}\"] [/graphic]", Path.GetFileNameWithoutExtension(ActiveDocument.Name));
            
            equationTag = String.Format(equationTag, graphicTag, mmlmathTag);
            markedRtb.Text = equationTag;
            markedRtb.SelectAll();
            markedRtb.SelectionFont = new System.Drawing.Font("Arial", 12, FontStyle.Regular);
            markedRtb.SelectionColor = Color.Black;

            foreach(KeyValuePair<String, Color> tag in colors){
                try
                {
                    tagExp = new Regex( "\\[/?" + tag.Key + ".*?\\]", options);
                    matchResults = tagExp.Match(markedRtb.Text);
                    while (matchResults.Success)
                    {
                        /* Buscamos y coloreamos el inicio o final de la etiqueta(tag) */
                        markedRtb.Select(matchResults.Index, matchResults.Length);
                        markedRtb.SelectionColor = tag.Value;
                        matchResults = matchResults.NextMatch();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }

            /*Remove equation and insert tags*/
            Word.Range rng = equation.Range;
            Clipboard.Clear();
            Clipboard.SetText(markedRtb.Rtf, TextDataFormat.Rtf);
            rng.Select();
            rng.Paste();
            Clipboard.Clear();
            /*Test native img convert*/
            /*equation.Range.Copy();
            object PDT = Word.WdPasteDataType.wdPasteEnhancedMetafile;
            rng.PasteSpecial(ref missing, ref missing, ref missing, ref missing, ref PDT, ref missing, ref missing);
            */
            equation.Remove();
            equationStart = equation.Range.Start - equationTag.Length - 1;
            rng.SetRange(equationStart, rng.End+1);
            Word.Find findObject = rng.Find;
            //findObject.ClearFormatting();
            findObject.Text = Environment.NewLine;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = "";
            object replaceAll = Word.WdReplace.wdReplaceAll;
            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);
            findObject.Text = "\n";
            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);
            findObject.Text = "\r";
            findObject.Execute(ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing);

            /* Convert mml to img */
            if (img_convert && jeuclidExist)
            {
                tagExp = new Regex("\\[graphic.*?\\]", options);
                matchResults = tagExp.Match(markedRtb.Text);
                if (matchResults.Success)
                    imgStart = equationStart + matchResults.Index + matchResults.Length;
                File.WriteAllText(mmlPath, mmlxml.OuterXml);
                Process p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WorkingDirectory = jeuclidPath;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = Path.Combine(jeuclidPath, @"bin\mml2xxx.bat");
                p.StartInfo.Arguments = String.Format(@"{0} {1} -scriptSizeMult 1.1", mmlPath, imgPath);
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;
                p.Start();

                p.WaitForExit();

                rng.SetRange(imgStart, imgStart+1);
                rng.InlineShapes.AddPicture(imgPath);

            }
        }
    }

}
