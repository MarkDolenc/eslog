using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eslog_1_6;

namespace eslog2_0.Models
{
    public class Mapper
    {
        public EslogData mapEslog(IzdaniRacunEnostavni eslog)
        {
            EslogData eslogData = new EslogData();

            eslogData.sender = new Sender();
            eslogData.receiver = new Receiver();
            eslogData.invoice = new Invoice();
            eslogData.referenceDocuments = new List<ReferenceDocument>();
            eslogData.invoiceItems = new List<InvoiceItem>();

            try
            {
                #region Sender & Receiver

                foreach(var data in eslog.Items.PodatkiPodjetja)
                {
                    switch (data)
                    {
                       
                    }
                }

                #endregion
                #region INVOICE

                eslogData.invoice.invoiceNumber = eslog.Items.GlavaRacuna.StevilkaRacuna;
                eslogData.invoice.invoiceType = eslog.Items.GlavaRacuna.VrstaRacuna;
                

                #endregion
                #region ReferenceDocuments
                
                foreach(var doc in eslog.Items.ReferencniDokumenti)
                {
                    ReferenceDocument referenceDocument = new ReferenceDocument();

                    referenceDocument.documentNo = doc.StevilkaDokumenta;
                    referenceDocument.type = doc.VrstaDokumenta;

                    eslogData.referenceDocuments.Add(referenceDocument);
                }

                #endregion
                #region InvoiceItems



                #endregion

            }
            catch (Exception e)
            {
                throw e;
            }


            return eslogData;
        }
    }
}
