using System;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RestSharp;
using RegexMarkup.Classes;
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
        private int offset = 47800;
        private int limit = 50;
        private string url = "http://biblat.unam.mx/api";
        private string requestParams = "institutions/find.json/slug/{slug}/country/{country}/limit/{limit}/offset/{offset}";
        private string searchString = "-";
        private string country = "-";
        private List<Country> countrys = new List<Country>();
        public static Word.Document ActiveDocument = null;
        public static Object missing = Type.Missing;
        private MarkupStruct aff = null;
        private Tags tags = Tags.Instance;
        private RegexMarkup objectRegexMarkup = RegexMarkup.Instance;
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
            this.dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dgGrid_RowPostPaint);
            foreach (Control c in dataGridView1.Controls){
                 if (c is VScrollBar){
                     vScrollBar = (VScrollBar)c;
                     vScrollBar.Scroll += new ScrollEventHandler(dataGridView1_Scroll);
                     vScrollBar.ValueChanged += new EventHandler(vScrollBar_ValueChanged);
                 }
            }
            this.getCountrys();
        }

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

        public void addRows() {
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

        public void getInstitutions(){
            var client = new RestClient(url);
            var request = new RestRequest(requestParams, Method.GET);
            request.AddUrlSegment("limit", limit.ToString());
            request.AddUrlSegment("offset", offset.ToString());
            request.AddUrlSegment("slug", this.searchString);
            request.AddUrlSegment("country", this.country);
            IRestResponse<List<Institution>> result = client.Execute<List<Institution>>(request);
            Console.WriteLine("restInstitutionButton_Click");
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

        public void getCountrys()
        {
            var client = new RestClient(url);
            var request = new RestRequest("institutions/country.json", Method.GET);
            IRestResponse<List<Country>> result = client.Execute<List<Country>>(request);
            Country all = new Country();
            all.slugPais = "-";
            all.e_100x = "Todos";
            countrys.Add(all);
            countrys.AddRange(result.Data);
            this.countryCbox.DisplayMember = "e_100x";
            this.countryCbox.ValueMember = "slugPais";
            this.countryCbox.DataSource = countrys;
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

        private void submitSearch_Click(object sender, EventArgs e)
        {
            this.slugSearch();
            this.country = this.countryCbox.SelectedValue.ToString();
            this.offset = 0;
            getInstitutions();
        }


        public void slugSearch(){
            searchString = this.textSearch.Text.ToLower();
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
            if (searchString == "") {
                searchString = "-";
            }
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
            Clipboard.SetText(aff.OriginalStr, TextDataFormat.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void setAffiliation() {
            String intitution = this.dataGridView1.SelectedCells[0].Value.ToString().Trim();
            String city = this.dataGridView1.SelectedCells[1].Value.ToString().Trim();
            String state = null;
            String country = this.dataGridView1.SelectedCells[2].Value.ToString().Trim();
            String original = null;
            String markup = null;

            Match match = Regex.Match(city, @"(.+?),\s(.+?)$", RegexOptions.Multiline | RegexOptions.Compiled);
            original = String.Format("{0}, {1}, {2}.", intitution, city, country);
            markup = String.Format("[aff id=\"\" orgname=\"{0}\"]{0}, [city]{1}[/city], [country]{2}[/country].[/aff]", intitution, city, country);
            if (match.Success) {
                city = match.Groups[1].Value;
                state = match.Groups[2].Value;
                markup = String.Format("[aff id=\"a0#\" orgname=\"{0}\"]{0}, [city]{1}[/city], [state]{2}[/state], [country]{3}[/country].[/aff]", intitution, city, state, country);
            }
            Word.Paragraph parrafo = ActiveDocument.Application.Selection.Paragraphs[1];
            object parrafoStart = parrafo.Range.Start;
            aff = new MarkupStruct(original, markup, ActiveDocument.Range(ref parrafoStart, ref parrafoStart), true, true);
            aff.MarkedRtb.Font = new Font("Arial", 11, FontStyle.Regular);
            this.colorRefTags("aff", 1);
        }

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
    }

    public class Institution
    {
        [DisplayName("Institución")] 
        public string e_100u { get; set; }
        [DisplayName("Ciudad")] 
        public string e_100w { get; set; }
        [DisplayName("País")] 
        public string e_100x { get; set; }
    }

    public class Country
    {
        [DisplayName("slug")]
        public string slugPais { get; set; }
        [DisplayName("País")]
        public string e_100x { get; set; }
    }
}
