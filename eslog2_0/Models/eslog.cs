using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace eslog2_0.Models
{
    public class eslog
    {

        public static string constructEslog(EslogData data)
        {
            string eslog = "";

            decimal sumInvoiceLine = 0;
            decimal sumTaxes = 0;
            decimal sumInvoiceLinetax = 0;

            foreach(var item in data.invoiceItems)
            {
                sumInvoiceLinetax += item.totalPrice;
                sumInvoiceLine += item.price;
                sumTaxes += item.vatAmount;
            }

            var invoiceDate = data.invoice.invoiceDate.ToString("yyyy-MM-dd");
            var dueDate = data.invoice.dueDate.ToString("yyyy-MM-dd");
            var serviceDate = data.invoice.serviceDate.ToString("yyyy-MM-dd");


            XNamespace xmlns = "urn:eslog:2.00";
            XNamespace xmlnsxsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            XNamespace nonamexsi = "eSLOG20_INVOIC_v200.xsd";



            XDocument document = new XDocument(
                new XElement("Invoice",
                    new XAttribute(XNamespace.Xmlns + "xsi", xmlnsxsi.NamespaceName), 
                    new XAttribute(xmlnsxsi + "noNamespaceSchemaLocation", nonamexsi),
                        new XElement("M_INVOIC", new XAttribute("Id", "data"),
                            new XElement("S_UNH",
                                new XElement("D_0062", data.invoice.invoiceNumber),
                                new XElement("C_S009",
                                    new XElement("D_0065", "INVOIC"),
                                    new XElement("D_0052", "D"),
                                    new XElement("D_0054", "01B"),
                                    new XElement("D_0051", "UN")
                                )
                            ),
                            new XElement("S_BGM",
                                new XElement("C_C002",
                                    new XElement("D_1001", data.invoice.invoiceType)
                                ),
                                new XElement("C_C106",
                                    new XElement("D_1004", data.invoice.invoiceNumber)
                                )
                            ),
                            new XElement("S_DTM",
                                new XElement("C_C507",
                                    new XElement("D_2005", "137"), //When the invoice was issued
                                    new XElement("D_2380", invoiceDate)
                                )
                            ),
                            new XElement("S_DTM",
                                new XElement("C_C507",
                                    new XElement("D_2005", "35"),  //When the invoice service was realised
                                    new XElement("D_2380", serviceDate)
                                )
                            ),
                            new XElement("S_DTM",  
                                new XElement("C_C507",
                                    new XElement("D_2005", "432"),  //Date to which the invoice should be paid
                                    new XElement("D_2380", dueDate)
                                )
                            ),
                            new XElement("S_FTX",
                                new XElement("D_4451", "DOC"),
                                new XElement("C_C107",
                                    new XElement("D_4441", "P1")
                                ),
                                new XElement("C_C108",
                                    new XElement("D_4440", "urn:cen.eu:en16931:2017")
                                )
                            ),
                            data.referenceDocuments.Select(x =>  //TRY to add all reference documents in here...
                            new XElement("G_SG1",
                                new XElement("S_RFF",
                                    new XElement("C_C506",
                                        new XElement("D_1153", x.type),
                                        new XElement("D_1154", x.documentNo)
                                    )
                                )
                            )),
                            new XElement("G_SG1",
                                new XElement("S_RFF",
                                    new XElement("C_C506",
                                        new XElement("D_1153", "PQ"),
                                        new XElement("D_1154", data.invoice.paymentReference)
                                    )
                                )
                            ),
                            new XElement("G_SG2",   //PAGE 59 of eslog 2.0 part 2! //BUYER INFORMATION --> RECEIVER
                                new XElement("S_NAD", 
                                    new XElement("D_3035", "BY"), 
                                    new XElement("C_C080",
                                        new XElement("D_3036", data.receiver.name)
                                    ),
                                    new XElement("C_C059",
                                        new XElement("D_3042", data.receiver.address)
                                    ),
                                    new XElement("D_3164", data.receiver.city),
                                    new XElement("D_3251", data.receiver.postCode),
                                    new XElement("D_3207", data.receiver.countryCode)
                                ),
                                new XElement("S_FII",
                                    new XElement("D_3035", "BB"),   //BUYER BANK IDENTIFICATION
                                    new XElement("C_C078",
                                        new XElement("D_3194", data.receiver.iban)
                                    ),
                                    new XElement("C_C088",
                                        new XElement("D_3433", data.receiver.bankBic)
                                    )
                                ),
                                new XElement("G_SG3", 
                                    new XElement("S_RFF", 
                                        new XElement("C_C506",
                                            new XElement("D_1153", "0199"),
                                            new XElement("D_1154", data.receiver.registrationNumber)
                                        )
                                    )
                                ),
                                new XElement("G_SG3",
                                    new XElement("S_RFF",
                                        new XElement("C_C506",
                                            new XElement("D_1153", "VA"),
                                            new XElement("D_1154", data.receiver.taxNumber)
                                        )
                                    )
                                )
                            ),
                            new XElement("G_SG2",   //PAGE 76 of eslog documentation SELLER INFORMATION --> SENDER
                                new XElement("S_NAD",
                                    new XElement("D_3035", "SE"),
                                    new XElement("C_C080",
                                        new XElement("D_3036", data.sender.name)
                                    ),
                                    new XElement("C_C059",
                                        new XElement("D_3042", data.sender.address)
                                    ),
                                    new XElement("D_3164", data.sender.city),
                                    new XElement("D_3251", data.sender.postCode),
                                    new XElement("D_3207", data.sender.countryCode)
                                ),
                                new XElement("S_FII",   //FINANCIAL INSTITUTION INFORMATION
                                    new XElement("D_3035", "RB"),
                                    new XElement("C_C078",
                                        new XElement("D_3194", data.sender.iban)
                                    ),
                                    new XElement("C_C088",
                                        new XElement("D_3433", data.sender.bankBic)
                                    )
                                ),
                                new XElement("G_SG3",   //REFERENCES
                                    new XElement("S_RFF",
                                        new XElement("C_C506",
                                            new XElement("D_1153", "0199"),
                                            new XElement("D_1154", data.sender.registrationNumber)
                                        )
                                    )
                                ),
                                new XElement("G_SG3",   //REFERENCES
                                    new XElement("S_RFF",
                                        new XElement("C_C506",
                                            new XElement("D_1153", "VA"),
                                            new XElement("D_1154", data.sender.taxNumber)
                                        )
                                    )
                                )
                            ),
                            new XElement("G_SG2",   //DELIVERY PARTY --> RECEIVER
                                new XElement("S_NAD",
                                    new XElement("D_3035", "DP"),
                                    new XElement("C_C080",
                                        new XElement("D_3036", data.receiver.name)
                                    ),
                                    new XElement("C_C059",
                                        new XElement("D_3042", data.receiver.address)
                                    ),
                                    new XElement("D_3164", data.receiver.city),
                                    new XElement("D_3251", data.receiver.postCode),
                                    new XElement("D_3207", data.receiver.countryCode)
                                ),
                                new XElement("S_FII",
                                    new XElement("D_3035", "BB"),   //BUYER BANK IDENTIFICATION
                                    new XElement("C_C078",
                                        new XElement("D_3194", data.receiver.iban)
                                    ),
                                    new XElement("C_C088",
                                        new XElement("D_3433", data.receiver.bankBic)
                                    )
                                ),
                                new XElement("G_SG3",
                                    new XElement("S_RFF",
                                        new XElement("C_C506",
                                            new XElement("D_1153", "0199"),
                                            new XElement("D_1154", data.receiver.registrationNumber)
                                        )
                                    )
                                ),
                                new XElement("G_SG3",
                                    new XElement("S_RFF",
                                        new XElement("C_C506",
                                            new XElement("D_1153", "VA"),
                                            new XElement("D_1154", data.receiver.taxNumber)
                                        )
                                    )
                                )
                            ),
                            new XElement("G_SG7",
                                new XElement("S_CUX",   //CURRENCY
                                    new XElement("C_C504",
                                        new XElement("D_6347", "2"),
                                        new XElement("D_6345", data.invoice.currency)
                                    )
                                )
                            ),
                            //new XElement("G_SG8",
                            //    new XElement("S_PAT",   //PAYMENT TERMS BASIS (PAGE 96)
                            //        new XElement("D_4272", "1")
                            //    )
                            //),
                            new XElement("G_SG8",
                                new XElement("S_PAT",   //PAYMENT TERMS BASIS (PAGE 96)
                                    new XElement("D_4272", "1")
                                ),
                                new XElement("S_DTM",   //PAYMENT DUE DATE
                                    new XElement("C_C507",
                                        new XElement("D_2005", "13"),
                                        new XElement("D_2380", dueDate)
                                    )
                                ),
                                new XElement("S_PAI",
                                    new XElement("C_C534",
                                        new XElement("D_4461", "42")
                                    )
                                )
                            ),
                            //new XElement("G_SG8",
                            //    new XElement("S_PAI",
                            //        new XElement("C_C534",
                            //            new XElement("D_4461", "42")
                            //        )
                            //    )
                            //),
                            /*
                             * INVOICE ITEMS
                             */
                            data.invoiceItems.Select(item =>
                            new XElement("G_SG26",
                                new XElement("S_LIN",
                                    new XElement("D_1082", "1")
                                ),
                                new XElement("S_PIA",
                                    new XElement("D_4347", "5"),
                                    new XElement("C_C212",
                                        new XElement("D_7140", item.itemText),
                                        new XElement("D_7143", "SA")
                                    )
                                ),
                                new XElement("S_IMD",
                                    new XElement("D_7077", "F"),
                                    new XElement("C_C273",
                                        new XElement("D_7008", item.itemText)
                                    )
                                ),
                                new XElement("S_QTY",
                                    new XElement("C_C186",
                                        new XElement("D_6063", "47"),
                                        new XElement("D_6060", item.quantity),
                                        new XElement("D_6411", "H87")
                                    )
                                ),
                                new XElement("G_SG27",
                                    new XElement("S_MOA",
                                        new XElement("C_C516",
                                            new XElement("D_5025", "203"),  //LINE ITEM AMOUNT
                                            new XElement("D_5004", item.price)
                                        )
                                    )
                                ),
                                new XElement("G_SG27",
                                    new XElement("S_MOA",
                                        new XElement("C_C516",
                                            new XElement("D_5025", "38"),   //INVOICE ITEM AMOUNT
                                            new XElement("D_5004", item.price + item.vatAmount)
                                        )
                                    )
                                ),
                                new XElement("G_SG29",
                                    new XElement("S_PRI",
                                        new XElement("C_C509",
                                            new XElement("D_5125", "AAA"),  //NET CALCULATION
                                            new XElement("D_5118", item.price - item.discountAmount)
                                        )
                                    )
                                ),
                                new XElement("G_SG29",
                                    new XElement("S_PRI",
                                        new XElement("C_C509",
                                            new XElement("D_5125", "AAB"),  //GROSS CALCULATION
                                            new XElement("D_5118", item.price)
                                        )
                                    )
                                ),
                                new XElement("G_SG34",
                                    new XElement("S_TAX",   //ITEM VAT BREAKDOWN
                                        new XElement("D_5283", "7"),
                                        new XElement("C_C241",
                                            new XElement("D_5153", "VAT")
                                        ),
                                        new XElement("C_C243", 
                                            new XElement("D_5278", item.vatPercent)
                                        ),
                                        new XElement("D_5305", item.vatCategory)    //VAT TYPE; standard, zero, exempt, reverse,... PAGE 128 of DOCUMENTATION
                                    ),
                                    new XElement("S_MOA",
                                        new XElement("C_C516",
                                            new XElement("D_5025", "125"),  //ITEM PRICE 
                                            new XElement("D_5004", item.price * item.quantity - item.discountAmount)
                                        )
                                    ),
                                    new XElement("S_MOA",
                                        new XElement("C_C516",
                                            new XElement("D_5025", "124"),  //ITEM VAT
                                            new XElement("D_5004", item.vatAmount)))
                                )
                            )
                        ),

                            /*
                             * END OF INVOICE ITEMS
                             */
                        new XElement("S_UNS",
                            new XElement("D_0081", "D")
                        ),
                        new XElement("G_SG50",
                            new XElement("S_MOA",
                                new XElement("C_C516",
                                    new XElement("D_5025", "79"),   //SUM OF AMOUNTS
                                    new XElement("D_5004", data.invoice.totalAmount )
                                )
                            )
                        ),
                        new XElement("G_SG50",
                            new XElement("S_MOA",
                                new XElement("C_C516",
                                    new XElement("D_5025", "389"),   //SUM OF AMOUNTS
                                    new XElement("D_5004", data.invoice.totalAmount)
                                )
                            )
                        ),
                        new XElement("G_SG50",
                            new XElement("S_MOA",
                                new XElement("C_C516",
                                    new XElement("D_5025", "388"),
                                    new XElement("D_5004", data.invoice.totalAmount + data.invoice.vatAmount)
                                )
                            )
                        ),
                        new XElement("G_SG50",
                            new XElement("S_MOA",
                                new XElement("C_C516",
                                    new XElement("D_5025", "9"),    //TOTAL AMOUNT TO PAY
                                    new XElement("D_5004", data.invoice.totalAmount + data.invoice.vatAmount)
                                )
                            )
                        ),
                        new XElement("G_SG50",
                            new XElement("S_MOA",
                                new XElement("C_C516",
                                    new XElement("D_5025", "176"), //SUM OF ALL TAXES
                                    new XElement("D_5004", data.invoice.vatAmount)
                                )
                            )
                        ),
                        new XElement("G_SG52",
                            new XElement("S_TAX",
                                new XElement("D_5283", "7"),
                                new XElement("C_C241",
                                    new XElement("D_5153", "VAT")
                                ),
                                new XElement("C_C243",
                                    new XElement("D_5278", data.invoice.vatPercentage)
                                ),
                                new XElement("D_5305", "S")  //INVOICE VAT CATEGORY
                            ),
                            new XElement("S_MOA",
                                new XElement("C_C516",
                                    new XElement("D_5025", "125"),
                                    new XElement("D_5004", data.invoice.totalAmount)
                                )
                            ),
                            new XElement("S_MOA",
                                new XElement("C_C516",
                                    new XElement("D_5025", "124"),
                                    new XElement("D_5004", data.invoice.vatAmount)
                                )
                            )
                        )
                    ))
            );

            eslog = document.ToString();

            return eslog;
        }

    }
}
