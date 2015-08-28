using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RestSharp;
using RegexMarkup.Classes;
using RegexMarkup.Structs;
using Word = Microsoft.Office.Interop.Word;

namespace RegexMarkup.Forms
{
    public partial class FindInstitution : Form
    {

        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static FindInstitution instance = null;
        static readonly object padlock = new object();

        public static FindInstitution Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new FindInstitution();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private VScrollBar vScrollBar;
        private bool scrollEvent = false;
        private int offset = 0;
        private int limit = 50;
        private String url = "http://biblat.unam.mx/api";
        private String requestParams = "institutions/find.json/slug/{slug}/country/{country}/limit/{limit}/offset/{offset}";
        private String searchString = "-";
        private String country = "-";
        private List<Country> countrys = new List<Country>();
        public static Word.Document ActiveDocument = null;
        public static Object missing = Type.Missing;
        private MarkupStruct aff = null;
        private Tags tags = Tags.Instance;
        private RegexMarkup objectRegexMarkup = RegexMarkup.Instance;
        private InstitutionParams objectInstitutionParams = InstitutionParams.Instance;
        private DTDSciELO dtdSciELO = DTDSciELO.Instance;
        /* Definimos y asignamos el arreglo de colores para las etiquetas */
        private Color[] colors = new Color[]{
                Color.DarkBlue,
                Color.Teal,
                Color.Gray,
                Color.Blue,
                Color.Violet};

        FindInstitution(){
            InitializeComponent();
            /* Icon */
            this.Icon = System.Drawing.Icon.FromHandle(global::RegexMarkup.Properties.Resources.institution.GetHicon());
            this.dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dgGrid_RowPostPaint);
            foreach (Control c in dataGridView1.Controls){
                 if (c is VScrollBar){
                     vScrollBar = (VScrollBar)c;
                     vScrollBar.Scroll += new ScrollEventHandler(dataGridView1_Scroll);
                     vScrollBar.ValueChanged += new EventHandler(vScrollBar_ValueChanged);
                 }
            }
            this.getCountrys();
            this.textSearch.KeyDown +=new KeyEventHandler(textSearch_KeyDown);
        }

        public void getCountrys()
        {
            var client = new RestClient(url);
            var request = new RestRequest("institutions/country.json", Method.GET);
            IRestResponse<List<Country>> result = client.Execute<List<Country>>(request);
            Country all = new Country();
            all.paisInstitucionSlug = "-";
            all.pais = "Todos";
            countrys.Add(all);
            countrys.AddRange(result.Data);
            this.countryCbox.DisplayMember = "pais";
            this.countryCbox.ValueMember = "paisInstitucionSlug";
            this.countryCbox.DataSource = countrys;
        }

        public void getInstitutions(){
            var client = new RestClient(url);
            var request = new RestRequest(requestParams, Method.GET);
            request.AddUrlSegment("limit", limit.ToString());
            request.AddUrlSegment("offset", offset.ToString());
            request.AddUrlSegment("slug", this.searchString);
            request.AddUrlSegment("country", this.country);
            IRestResponse<List<Institution>> result = client.Execute<List<Institution>>(request);
            DataTable dt = new DataTable();
            dt = result.Data.ToDataTable();
            this.scrollEvent = true;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            foreach (DataColumn col in dt.Columns)
            {
                var c = new DataGridViewTextBoxColumn() { HeaderText = col.ColumnName };
                dataGridView1.Columns.Add(c);
            }
            foreach (DataRow dr in dt.Rows)
            {
                dataGridView1.Rows.Add(dr.ItemArray);
            }
            scrollEvent = false;
        }

        public void addRows()
        {
            var client = new RestClient(url);
            var request = new RestRequest(requestParams, Method.GET);
            offset += limit;
            request.AddUrlSegment("limit", limit.ToString());
            request.AddUrlSegment("offset", offset.ToString());
            request.AddUrlSegment("slug", this.searchString);
            request.AddUrlSegment("country", this.country);
            IRestResponse<List<Institution>> result = client.Execute<List<Institution>>(request);
            DataTable dt = new DataTable();
            dt = result.Data.ToDataTable();
            foreach (DataRow dr in dt.Rows)
            {
                dataGridView1.Rows.Add(dr.ItemArray);
            }
        }

        private void submit()
        {
            this.slugSearch();
            this.country = this.countryCbox.SelectedValue.ToString();
            this.offset = 0;
            getInstitutions();
        }

        #region slugSearch
        public void slugSearch()
        {
            searchString = this.textSearch.Text;
            if (Regex.IsMatch(searchString, "[A-Z]+?$", RegexOptions.Multiline))
            {
                searchString = Regex.Replace(searchString, "([A-Z])", "$1.", RegexOptions.Multiline);
            }
            searchString = searchString.ToLower();
            Dictionary<char, char> lReplaceValues = new Dictionary<char, char>();
            lReplaceValues['á'] = 'a';
            lReplaceValues['é'] = 'e';
            lReplaceValues['í'] = 'i';
            lReplaceValues['ó'] = 'o';
            lReplaceValues['ú'] = 'u';
            lReplaceValues['à'] = 'a';
            lReplaceValues['è'] = 'e';
            lReplaceValues['ì'] = 'i';
            lReplaceValues['ò'] = 'o';
            lReplaceValues['ù'] = 'u';
            lReplaceValues['â'] = 'a';
            lReplaceValues['ê'] = 'e';
            lReplaceValues['î'] = 'i';
            lReplaceValues['ô'] = 'o';
            lReplaceValues['û'] = 'u';
            lReplaceValues['ä'] = 'a';
            lReplaceValues['ë'] = 'e';
            lReplaceValues['ï'] = 'i';
            lReplaceValues['ö'] = 'o';
            lReplaceValues['ü'] = 'u';
            lReplaceValues['å'] = 'a';
            lReplaceValues['ů'] = 'u';
            lReplaceValues['ā'] = 'a';
            lReplaceValues['ē'] = 'e';
            lReplaceValues['ī'] = 'i';
            lReplaceValues['ō'] = 'o';
            lReplaceValues['ū'] = 'u';
            lReplaceValues['ă'] = 'a';
            lReplaceValues['ĕ'] = 'e';
            lReplaceValues['ĭ'] = 'i';
            lReplaceValues['ŏ'] = 'o';
            lReplaceValues['ŭ'] = 'u';
            lReplaceValues['ą'] = 'a';
            lReplaceValues['ã'] = 'a';
            lReplaceValues['ę'] = 'e';
            lReplaceValues['ė'] = 'e';
            lReplaceValues['ě'] = 'e';
            lReplaceValues['õ'] = 'o';
            lReplaceValues['ő'] = 'o';
            lReplaceValues['ø'] = 'o';
            lReplaceValues['ũ'] = 'u';
            lReplaceValues['ĩ'] = 'i';
            lReplaceValues['&'] = '-';
            lReplaceValues[','] = '-';
            lReplaceValues['.'] = '-';
            lReplaceValues['ç'] = 'c';
            lReplaceValues['š'] = 's';
            lReplaceValues['ş'] = 's';
            lReplaceValues['ž'] = 'z';
            lReplaceValues['ý'] = 'y';
            lReplaceValues['ÿ'] = 'y';
            lReplaceValues['ß'] = 'b';
            lReplaceValues['þ'] = 'p';
            lReplaceValues['ñ'] = 'n';

            foreach (KeyValuePair<char, char> lReplaceValue in lReplaceValues)
            {
                searchString = searchString.Replace(lReplaceValue.Key, lReplaceValue.Value);
            }
            searchString = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(searchString));
            searchString = Regex.Replace(searchString, @"\s", "-", RegexOptions.Compiled);
            searchString = Regex.Replace(searchString, @"[^\w-]", "", RegexOptions.Compiled);
            searchString = Regex.Replace(searchString, @"-+", "-", RegexOptions.Compiled);
            searchString = Regex.Replace(searchString, @"(-$|^-)", "", RegexOptions.Compiled);
            if (searchString == "")
            {
                searchString = "-";
            }
        }
        #endregion

        #region setAffiliation
        private void setAffiliation() {
            String intitution = this.dataGridView1.SelectedCells[0].Value.ToString().Trim();
            String city = this.dataGridView1.SelectedCells[1].Value.ToString().Trim();
            String state = null;
            String country = this.dataGridView1.SelectedCells[2].Value.ToString().Trim();
            String id = "a0#";
            String orgdiv1 = "";
            String orgdiv2 = "";
            String orgdiv3 = "";
            String zipcode = "";
            String email = "";
            String orgdiv1Markup = "";
            String orgdiv2Markup = "";
            String orgdiv3Markup = "";
            String zipcodeMarkup = "";
            String emailMarkup = "";
            String original = null;
            String markup = null;

            this.Hide();

            if (this.checkBoxCompletInfo.Checked) {
                objectInstitutionParams.OriginalAffiliation = ActiveDocument.Application.Selection.Paragraphs[1].Range.Text;
                objectInstitutionParams.ShowDialog();
                id = objectInstitutionParams.Id;
                orgdiv1 = objectInstitutionParams.OrgDiv1;
                orgdiv2 = objectInstitutionParams.OrgDiv2;
                orgdiv3 = objectInstitutionParams.OrgDiv3;
                zipcode = objectInstitutionParams.ZipCode;
                email = objectInstitutionParams.Email;
            }
            if (orgdiv1 != "")
            {
                orgdiv1Markup = String.Format(" orgdiv1=\"{0}\"", orgdiv1.Replace("\"", ""));
                orgdiv1 = String.Format(" {0},", orgdiv1);
            }
            if (orgdiv2 != "")
            {
                orgdiv2Markup = String.Format(" orgdiv2=\"{0}\"", orgdiv2.Replace("\"", ""));
                orgdiv2 = String.Format(" {0},", orgdiv2);
            }
            if (orgdiv3 != "")
            {
                orgdiv3Markup = String.Format(" orgdiv3=\"{0}\"", orgdiv3.Replace("\"", ""));
                orgdiv3 = String.Format(" {0},", orgdiv3);
            }
            if (zipcode != "")
            {
                zipcodeMarkup = String.Format(" [zipcode]{0}[/zipcode],", zipcode);
                zipcode = String.Format(" {0},", zipcode);
            }
            if (email != "")
            {
                emailMarkup = String.Format(" [email]{0}[/email].", email);
                email = String.Format(" {0}.", email);
            }

            Match match = Regex.Match(city, @"(.+?),\s(.+?)$", RegexOptions.Multiline | RegexOptions.Compiled);
            original = String.Format("{0},{3}{4}{5}{6} {1}, {2}.{7}", intitution, city, country, orgdiv1, orgdiv2, orgdiv3, zipcode, email);
            markup = String.Format("[aff id=\"{0}\" orgname=\"{12}\"{4}{5}{6}]{1},{7}{8}{9}{10} [city]{2}[/city], [country]{3}[/country].{11}[/aff]", id, intitution, city, country, orgdiv1Markup, orgdiv2Markup, orgdiv3Markup, orgdiv1, orgdiv2, orgdiv3, zipcodeMarkup, emailMarkup, intitution.Replace("\"", ""));
            if (match.Success) {
                city = match.Groups[1].Value;
                state = match.Groups[2].Value;
                markup = String.Format("[aff id=\"{0}\" orgname=\"{13}\"{5}{6}{7}]{1},{8}{9}{10}{11} [city]{2}[/city], [state]{3}[/state], [country]{4}[/country].{12}[/aff]", id, intitution, city, state, country, orgdiv1Markup, orgdiv2Markup, orgdiv3Markup, orgdiv1, orgdiv2, orgdiv3, zipcodeMarkup, emailMarkup, intitution.Replace("\"", ""));
            }
            Word.Paragraph parrafo = ActiveDocument.Application.Selection.Paragraphs[1];
            object parrafoStart = parrafo.Range.Start;
            aff = new MarkupStruct(original, markup, ActiveDocument.Range(ref parrafoStart, ref parrafoStart), true, true);
            aff.MarkedRtb.Font = new Font("Arial", 11, FontStyle.Regular);
            this.colorRefTags("aff", 1);
        }
        #endregion

        #region colorTags
        /// <summary>
        /// Function to colorize tags in form
        /// </summary>
        /// <param name="structNode">Contenido de la etiqueta actual con sus hijos</param>
        /// <param name="color">Color de la etiqueta {0,...,4} el -1 sirver para iniciar </param>
        public void colorRefTags(String node, int color)
        {
            Regex tagExp = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            Match matchResults = null;
            String startTag = null;
            bool TagSuccess = false;
            /* Si el color inicial es -1 formateamos el texto con la fuente estandar y asignamos color=0 */
            if (color == -1)
            {
                this.aff.MarkedRtb.SelectAll();
                this.aff.MarkedRtb.SelectionFont = new Font("Verdana", 10, FontStyle.Regular);
                this.aff.MarkedRtb.SelectionColor = Color.Black;
                color = 0;
            }
            else
            {
                /* Iteramos las etiquetas(tags) hijas*/
                color++;
            }
            /* Si el indice de color llego a 20 lo reiniciamos a 0 */
            color = color == 5 ? 0 : color;
            /*Coloreamos el nodo principal*/
            tagExp = new Regex("\\[/*" + node + ".*?\\]", options);
            matchResults = tagExp.Match(this.aff.MarkedRtb.Text);
            while (matchResults.Success)
            {
                startTag = matchResults.Value;

                /* Buscamos y coloreamos el inicio o final de la etiqueta(tag) */
                this.aff.MarkedRtb.Select(matchResults.Index, matchResults.Length);
                this.aff.MarkedRtb.SelectionFont = new Font("Arial", 11, FontStyle.Regular);
                this.aff.MarkedRtb.SelectionColor = this.colors[color];
                TagSuccess = true;
                matchResults = matchResults.NextMatch();
            }
            /*Coloreamos los nodos hijos*/
            color++;
            color = color == 5 ? 0 : color;
            foreach (String tag in this.tags.getChilds(node))
            {
                try
                {
                    tagExp = new Regex("\\[/*" + tag + ".*?\\]", options);
                    matchResults = tagExp.Match(this.aff.MarkedRtb.Text);
                    while (matchResults.Success)
                    {
                        startTag = matchResults.Value;

                        /* Buscamos y coloreamos el inicio o final de la etiqueta(tag) */
                        this.aff.MarkedRtb.Select(matchResults.Index, matchResults.Length);
                        this.aff.MarkedRtb.SelectionFont = new Font("Arial", 11, FontStyle.Regular);
                        this.aff.MarkedRtb.SelectionColor = this.colors[color];
                        TagSuccess = true;
                        matchResults = matchResults.NextMatch();
                    }
                    /* Si la etiqueta fue coloreada con exito coloreamos y el nodo tiene etiquetas hijas */
                    if (TagSuccess && this.tags.Tag[tag].ChildNodes)
                    {
                        this.colorRefTags(tag, color);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        #endregion

        #region generalEvents
        void vScrollBar_ValueChanged(object sender, object e)
        {
            int scrollMax = vScrollBar.Maximum - vScrollBar.Size.Height;
            if (!scrollEvent && vScrollBar.Value >= scrollMax)
            {
                this.addRows();
            }
        }

        void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            int scrollMax = vScrollBar.Maximum - vScrollBar.Size.Height;
            scrollEvent = true;
            if (e.Type == ScrollEventType.EndScroll)
            {
                if (vScrollBar.Value >= scrollMax)
                {
                    this.addRows();
                }
                scrollEvent = false;
            }
        }

        private void dgGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void FindInstitution_Load(object sender, EventArgs e)
        {
            /* Inicializamos variables */
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            this.tags.Dtd = dtdSciELO.getDTD("4.0", "article");
            this.textSearch.Text = ActiveDocument.Application.Selection.Text.Trim();
            this.offset = 0;
            this.slugSearch();
            this.getInstitutions();
        }
        #endregion

        #region clickEvents

        void textSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                submit();
            }
        }

        private void submitSearch_Click(object sender, EventArgs e)
        {
            submit();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            this.setAffiliation();
            object parrafoStart = aff.RngCita.Start;
            object parrafoEnd = ((int)parrafoStart + aff.MarkedStr.Length);
            object idAffStart = ((int)parrafoStart + "[aff id=\"a0".Length);
            object idAffEnd = ((int)idAffStart + 1);
            Clipboard.Clear();
            aff.MarkedRtb.SelectAll();
            Clipboard.SetText(aff.MarkedRtb.SelectedRtf, TextDataFormat.Rtf);
            aff.RngCita.Paste();
            ActiveDocument.Range(ref parrafoStart, ref parrafoEnd).InsertParagraphAfter();
            ActiveDocument.Range(ref idAffStart, ref idAffEnd).Select();
            Clipboard.Clear();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            this.setAffiliation();
            object parrafoStart = aff.RngCita.Start;
            object parrafoEnd = ((int)parrafoStart + aff.OriginalStr.Length);
            Clipboard.Clear();
            aff.MarkedRtb.SelectAll();
            Clipboard.SetText(aff.OriginalStr, TextDataFormat.Text);
            aff.RngCita.Paste();
            ActiveDocument.Range(ref parrafoStart, ref parrafoEnd).InsertParagraphAfter();
            ActiveDocument.Range(ref parrafoStart, ref parrafoStart).Select();
            Clipboard.Clear();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion
    }
}
