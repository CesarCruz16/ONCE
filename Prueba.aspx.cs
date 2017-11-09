using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
//using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Runtime.InteropServices;

using System.Reflection;
using System.Configuration;

using Word = Microsoft.Office.Interop.Word;

public partial class Prueba : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
              
        }
        
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //Word.Application objWordApp;
        //objWordApp = new Word.Application();
        //Word.Document objDoc;
        //string rng = "Homero 213";
        //// Open an existing document.
        //objWordApp.Documents.Open("C:\\envio\\Muestra.docx");
        //objDoc = objWordApp.ActiveDocument;
        //// Find and replace some text.
        //objDoc.Content.Find.Execute("<Address>", rng, Word.WdReplace.wdReplaceAll);
        //// Save and close the document.
        //objWordApp.Documents.Save();
        //objWordApp.Documents.Close(Word.WdSaveOptions.wdDoNotSaveChanges);
        ////objWordApp.Quit();
        //objWordApp = null;

        object o = Missing.Value;
        object oFalse = false;
        object oTrue = true;

        Word._Application app = null;
        Word.Documents docs = null;
        Word.Document doc = null;

        object path = @"C:\pru\otrox.doc";

        try
        {
            app = new Word.Application();
            app.Visible = false;
            app.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;

            docs = app.Documents;
            //doc = docs.Open(ref path, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o);
            // segundo doc = docs.Open(ref path, ref o, false, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o);

            doc = docs.Open(ref path, Missing.Value, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


            doc.Activate();

            foreach (Word.Range range in doc.StoryRanges)
            {
                Word.Find find = range.Find;
                object findText = "$cliente_representante_legal;";
                object replacText = "Espergencio Rocha Carreño";
                object replace = Word.WdReplace.wdReplaceAll;
                object findWrap = Word.WdFindWrap.wdFindContinue;

                find.Execute(ref findText, ref o, ref o, ref o, ref oFalse, ref o,
                    ref o, ref findWrap, ref o, ref replacText,
                    ref replace, ref o, ref o, ref o, ref o);

                Marshal.FinalReleaseComObject(find);
                Marshal.FinalReleaseComObject(range);
            }
            doc.SaveAs2(@"C:\pru\Modif.doc");
            //doc.Save();
            ((Word._Document)doc).Close(ref o, ref o, ref o);
            app.Quit(ref o, ref o, ref o);
        }
        finally
        {
            if (doc != null)
                Marshal.FinalReleaseComObject(doc);

            if (docs != null)
                Marshal.FinalReleaseComObject(docs);

            if (app != null)
                Marshal.FinalReleaseComObject(app);
        }

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //string parametro = "";
        //string oUrl = "http://egenium.org/TenMas/Word.aspx?dato=NOMBRE" + parametro;

        string oUrl = "Muestra.aspx?dato=otro.doc"; 

        string sScript = "<script language =javascript> ";
        sScript += "window.open('" + oUrl + "',null,'toolbar=0,scrollbars=1,location=no,statusbar=0,menubar=0,resizable=1,width=500,height=300,left=100,top=100');";
        sScript += "</script> ";
        Response.Write(sScript); 
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        object o = Missing.Value;
        object oFalse = false;
        object oTrue = true;

        Word._Application app = null;
        Word.Documents docs = null;
        Word.Document doc = null;

        object path = @"C:\pru\otro.doc";

        try
        {
            app = new Word.Application();
            app.Visible = true;
            app.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone;

            docs = app.Documents;
            //doc = docs.Open(ref path, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o);
            // segundo doc = docs.Open(ref path, ref o, false, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o, ref o);
            //doc = docs.Open(ref path, Missing.Value, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

           doc = docs.Open(ref path, Missing.Value, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

           doc.Activate();
        }
        finally
        {
            if (doc != null)
                Marshal.FinalReleaseComObject(doc);

            if (docs != null)
                Marshal.FinalReleaseComObject(docs);

            if (app != null)
                Marshal.FinalReleaseComObject(app);
        }
    }
}