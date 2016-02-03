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

        public void initialize() {
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            jeuclidExist = File.Exists(Path.Combine(jeuclidPath, "bin/mml2xxx.bat"));
        }
        
        public void convertSelection() {
            Word.Selection docSeleccion = Globals.ThisAddIn.Application.Selection;
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
            if (ActiveDocument.OMaths.Count <= 0)
            {
                MessageBox.Show("No se encontraron ecuaciones");
                return;
            }
            DialogResult result;
            

            result = MessageBox.Show(String.Format("Se encontraron {0} ecuaciones.\n Continuar?", ActiveDocument.OMaths.Count), "", MessageBoxButtons.YesNo);

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
            /* Mostramos de nuevo la aplicacion */
            waitForm.Hide();
            Globals.ThisAddIn.Application.Visible = true;
        }

        public void revertAll(){

        }

        public void convert(OMath equation) {
            this.convert(equation, true, true);
        }

        public void convert(OMath equation, Boolean mml_convert, Boolean img_convert) {
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
            mmlMarkup = writer.ToString().Replace("[mml:math]", "[mml:math xmlns:mml=\"http://www.w3.org/1998/Math/MathML\"]"); ;
            mmlMarkup = writer.ToString().Replace("[mml:math]", "[mml:math xmlns:mml=\"http://www.w3.org/1998/Math/MathML\"]");

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
            Clipboard.Clear();
            Clipboard.SetText(markedRtb.Rtf, TextDataFormat.Rtf);
            Word.Range rng = ActiveDocument.Range(equation.Range.Start, equation.Range.End);
            rng.Delete();
            equation.Remove();
            rng.Paste();
            Clipboard.Clear();
            equationStart = equation.Range.Start - equationTag.Length - 1;

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
                p.Start();

                string output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                rng = ActiveDocument.Range(imgStart, imgStart+1);
                rng.InlineShapes.AddPicture(imgPath);
            }
        }

    }
}
