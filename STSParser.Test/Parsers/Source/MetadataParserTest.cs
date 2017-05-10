using HtmlAgilityPack;
using NUnit.Framework;
using STSParser.Parsers;

namespace STSParser.Test.Parsers.Source
{
    [TestFixture]
    public class MetadataParserTest
    {
        [SetUp]
        public void Setup()
        {
            ItemMetadataTable = HtmlNode.CreateNode(
                @"<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='border-collapse: collapse'>
                <tr style='page-break-inside: avoid'>
                    <td width=169 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 126.9pt;'>
                        <p class=ItemCard align=right style='text-align: right'>
                            <span lang=FR>
                                Item
                                Code:
                            </span>
                        </p>
                    </td>
                    <td width=145 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 108.5pt;'>
                        <p class=ItemCard>
                            <span lang=FR>XTR30636.OSA</span>
                        </p>
                    </td>
                    <td width=125 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 94.0pt;'>
                        <p class=ItemCard align=right style='text-align: right'>Passage Title:</p>
                    </td>
                    <td width=271 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 203.4pt;'>
                        <p class=ItemCard>&nbsp;</p>
                    </td>
                </tr>
                <tr style='page-break-inside: avoid'>
                    <td width=169 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 126.9pt;'>
                        <p class=ItemCard align=right style='text-align: right'>DOK:</p>
                    </td>
                    <td width=145 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 108.5pt;'>
                        <p class=ItemCard>1</p>
                    </td>
                    <td width=125 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 94.0pt;'>
                        <p class=ItemCard align=right style='text-align: right'>
                            <span lang=FR>
                                Passage
                                Code:
                            </span>
                        </p>
                    </td>
                    <td width=271 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 203.4pt;'>
                        <p class=ItemCard>
                            <span lang=FR>&nbsp;</span>
                        </p>
                    </td>
                </tr>
                <tr style='page-break-inside: avoid'>
                    <td width=169 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 126.9pt;'>
                        <p class=ItemCard align=right style='text-align: right'>
                            <span lang=FR>
                                Standard
                                Code:
                            </span>
                        </p>
                    </td>
                    <td width=541 colspan=3 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 405.9pt;'>
                        <p class=ItemCard>
                            <span lang=FR>9WC1.5.A</span>
                        </p>
                    </td>
                </tr>
                <tr style='page-break-inside: avoid'>
                    <td width=169 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 126.9pt;'>
                        <p class=ItemCard align=right style='text-align: right'>
                            <span lang=FR>Strand:</span>
                        </p>
                    </td>
                    <td width=541 colspan=3 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 405.9pt;'>
                        <p class=ItemCard>
                            <span lang=FR>&nbsp;</span>
                        </p>
                    </td>
                </tr>
                <tr style='page-break-inside: avoid'>
                    <td width=169 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 126.9pt;'>
                        <p class=ItemCard align=right style='text-align: right'>Report Category:</p>
                    </td>
                    <td width=541 colspan=3 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 405.9pt;'>
                        <p class=ItemCard>1.0 WRITTEN AND ORAL ENGLISH LANGUAGE CONVENTIONS</p>
                    </td>
                </tr>
                <tr style='page-break-inside: avoid'>
                    <td width=169 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 126.9pt;'>
                        <p class=ItemCard align=right style='text-align: right'>Standard:</p>
                    </td>
                    <td width=541 colspan=3 valign=top style='padding: 0in 5.4pt 0in 5.4pt; width: 405.9pt;'>
                        <p class=MsoNormal style='margin-left: 1.1pt; text-indent: -1.1pt'>
                            <span
                                style='font-family: 'Arial', sans-serif; font-size: 11.0pt;'>
                                1.5 Manuscript Form:
                                reflect appropriate manuscript requirements, including
                            </ span >
                        </ p >
                        < p class=MsoNormal>
                            <span style = 'font-family: 'Arial', sans-serif; font-size: 11.0pt;' >
                                a.
                                title page presentation
                            </span>
                        </p>
                    </td>
                </tr>
                <tr style = 'page-break-inside: avoid' >
                    < td width=169 valign=top style = 'padding: 0in 5.4pt 0in 5.4pt; width: 126.9pt;' >
                         < p class=ItemCard align = right style='text-align: right'>Correct Answer:</p>
                    </td>
                    <td width = 541 colspan=3 valign=top style = 'padding: 0in 5.4pt 0in 5.4pt; width: 405.9pt;' >  
                          < p class=ItemCard>B</p>
                    </td>
                </tr>
            </table>");
        }

        public HtmlNode ItemMetadataTable { get; set; }

        [Test]
        public void ValidItemTableProducesCorrectMetadata()
        {
            // Arrange
            // Act
            var result = ItemMetadataParser.Parse(ItemMetadataTable);

            // Assert
            Assert.IsTrue(result.ContainsKey("ItemCode"));
            Assert.AreEqual(result["ItemCode"], "XTR30636.OSA");
            Assert.IsTrue(result.ContainsKey("DOK"));
            Assert.AreEqual(result["DOK"], "1");
            Assert.IsTrue(result.ContainsKey("StandardCode"));
            Assert.AreEqual(result["StandardCode"], "9WC1.5.A");
            Assert.IsTrue(result.ContainsKey("ReportCategory"));
            Assert.AreEqual(result["ReportCategory"], "1.0 WRITTEN AND ORAL ENGLISH LANGUAGE CONVENTIONS");
            Assert.IsTrue(result.ContainsKey("CorrectAnswer"));
            Assert.AreEqual(result["CorrectAnswer"], "B");
            Assert.IsTrue(result.ContainsKey("Standard"));
            Assert.AreEqual(result["Standard"],
                "1.5 Manuscript Form: reflect appropriate manuscript requirements, including a. title page presentation");
        }
    }
}