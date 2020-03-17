using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Eslog_1_6;

namespace eslog2_0.Models
{
    public class Mapper
    {


        private decimal GetDecimalFromString(string value)
        {
            decimal retVal = decimal.Zero;

            if (decimal.TryParse(value, NumberStyles.Currency, new CultureInfo("sl-SI"), out retVal))
                return retVal;


            return retVal;

        }


        /// <summary>
        /// Maps the existing eslog 1.6 format to our custom data model for eslog 2.0
        /// </summary>
        /// <param name="eslog"></param>
        /// <returns></returns>
        public EslogData mapEslog(IzdaniRacunEnostavni eslog)
        {
            EslogData eslogData = new EslogData();

            eslogData.sender = new Sender();
            eslogData.receiver = new Receiver();
            eslogData.invoice = new Invoice();
            eslogData.referenceDocuments = new List<ReferenceDocument>();
            eslogData.invoiceItems = new List<InvoiceItem>();

            var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            var provider = new CultureInfo("sl-SI");


            try
            {
                #region Sender & Receiver

                foreach(var data in eslog.Items.PodatkiPodjetja)
                {
                    switch (data.NazivNaslovPodjetja.VrstaPartnerja)
                    {
                        case "BY":
                            eslogData.receiver.name = data.NazivNaslovPodjetja.NazivPartnerja.NazivPartnerja1;
                            eslogData.receiver.longname = data.NazivNaslovPodjetja.NazivPartnerja.NazivPartnerja1;
                            eslogData.receiver.address = data.NazivNaslovPodjetja.Ulica.Ulica1;
                            eslogData.receiver.city = data.NazivNaslovPodjetja.Kraj;
                            eslogData.receiver.country = data.NazivNaslovPodjetja.NazivDrzave;
                            eslogData.receiver.postCode = data.NazivNaslovPodjetja.PostnaStevilka;
                            eslogData.receiver.countryCode = data.NazivNaslovPodjetja.KodaDrzave;
                            eslogData.receiver.iban = data.FinancniPodatkiPodjetja[0].StevilkaBancnegaRacuna;
                            eslogData.receiver.bankBic = data.FinancniPodatkiPodjetja[0].BIC;

                            foreach(var value in data.ReferencniPodatkiPodjetja)
                            {
                                switch (value.VrstaPodatkaPodjetja)
                                {
                                    case "GN":
                                        eslogData.receiver.registrationNumber = value.PodatekPodjetja;
                                        break;

                                    case "VA":
                                        eslogData.receiver.taxNumber = value.PodatekPodjetja;
                                        break;

                                    default:
                                        break;
                                }
                            }

                            break;

                        case "II":
                            eslogData.sender.name = data.NazivNaslovPodjetja.NazivPartnerja.NazivPartnerja1;
                            eslogData.sender.longname = data.NazivNaslovPodjetja.NazivPartnerja.NazivPartnerja1;
                            eslogData.sender.address = data.NazivNaslovPodjetja.Ulica.Ulica1;
                            eslogData.sender.city = data.NazivNaslovPodjetja.Kraj;
                            eslogData.sender.country = data.NazivNaslovPodjetja.NazivDrzave;
                            eslogData.sender.postCode = data.NazivNaslovPodjetja.PostnaStevilka;
                            eslogData.sender.countryCode = data.NazivNaslovPodjetja.KodaDrzave;
                            eslogData.sender.iban = data.FinancniPodatkiPodjetja[0].StevilkaBancnegaRacuna;
                            eslogData.sender.bankBic = data.FinancniPodatkiPodjetja[0].BIC;

                            foreach (var value in data.ReferencniPodatkiPodjetja)
                            {
                                switch (value.VrstaPodatkaPodjetja)
                                {
                                    case "GN":
                                        eslogData.sender.registrationNumber = value.PodatekPodjetja;
                                        break;

                                    case "VA":
                                        eslogData.sender.taxNumber = value.PodatekPodjetja;
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }

                #endregion

                #region INVOICE
                
                eslogData.invoice.invoiceNumber = eslog.Items.GlavaRacuna.StevilkaRacuna;
                eslogData.invoice.invoiceType = eslog.Items.GlavaRacuna.VrstaRacuna;
                eslogData.invoice.paymentTerms = eslog.Items.PlacilniPogoji[0].PodatkiORokih.VrstaPogoja;
                eslogData.invoice.dueDate = DateTime.Parse(eslog.Items.PlacilniPogoji[0].PlacilniRoki.Datum);
                eslogData.invoice.currency = eslog.Items.Valuta[0].KodaValute;

                //INVOICE AMOUNTS
                foreach (var amount in eslog.Items.PovzetekZneskovRacuna)
                {
                    switch (amount.ZneskiRacuna.VrstaZneska)
                    {
                        case "124":
                            eslogData.invoice.vatAmount = GetDecimalFromString(amount.ZneskiRacuna.ZnesekRacuna);
                            break;

                        case "125":
                            eslogData.invoice.totalAmount = GetDecimalFromString(amount.ZneskiRacuna.ZnesekRacuna);
                            break;

                        case "9":
                            eslogData.invoice.paymentAmount = GetDecimalFromString(amount.ZneskiRacuna.ZnesekRacuna);
                            break;

                    }

                    if(amount.SklicZaPlacilo.StevilkaSklica != null || !String.IsNullOrEmpty(amount.SklicZaPlacilo.StevilkaSklica))
                    {
                        eslogData.invoice.paymentReference = amount.SklicZaPlacilo.StevilkaSklica;
                    }
                }

                

                foreach(var date in eslog.Items.DatumiRacuna)
                {
                    switch (date.VrstaDatuma)
                    {
                        case "137":
                            eslogData.invoice.invoiceDate = DateTime.Parse(date.DatumRacuna);
                            break;

                        case "263":
                            eslogData.invoice.serviceDate = DateTime.Parse(date.DatumRacuna);
                            break;

                        case "35":
                            break;
                    }
                }
                

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

                foreach(var item in eslog.Items.PostavkeRacuna)
                {
                    InvoiceItem invoiceItem = new InvoiceItem();

                    invoiceItem.itemText = item.OpisiArtiklov[0].OpisArtikla.OpisArtikla1;
                    invoiceItem.quantity = int.Parse(item.KolicinaArtikla.Kolicina);
                    
                    foreach(var amm in item.ZneskiPostavke)
                    {
                        switch (amm.VrstaZneskaPostavke)
                        {
                            case "203":
                                invoiceItem.price = GetDecimalFromString(amm.ZnesekPostavke);
                                break;

                            case "38":
                                invoiceItem.totalPrice = GetDecimalFromString(amm.ZnesekPostavke);
                                break;

                            default:
                                break;
                        }
                    }
                    invoiceItem.vatPercent = GetDecimalFromString(item.DavkiPostavke.DavkiNaPostavki.OdstotekDavkaPostavke);
                    invoiceItem.vatType = item.DavkiPostavke.DavkiNaPostavki.VrstaDavkaPostavke;
                    
                    foreach(var vat in item.DavkiPostavke.ZneskiDavkovPostavke)
                    {
                        switch (vat.VrstaZneskaDavkaPostavke)
                        {
                            case "124":
                                invoiceItem.vatAmount = GetDecimalFromString(vat.Znesek);
                                break;

                            default:
                                break;
                        }
                    }

                    invoiceItem.vatCategory = "S";

                    eslogData.invoiceItems.Add(invoiceItem);

                }

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
