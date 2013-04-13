<%@ Reference Page="~/members/link-page-design/default.aspx" %>

<%@ Page Language="c#" MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>
<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>
<asp:Content runat="Server" ContentPlaceHolderID="mainContent">

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/Global.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/DynamicHelp.js"></script>

    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Members/Link-Page-Design/LinkPageDesign.js"></script>

    <p style="text-align: center">
        <b>NaturalLinkExchange <b>Free</b> eBooks</b></p>
    <p>
        Information is what the Internet is all about - I don't know about you but if the
        title is good I usually can't wait to read an ebook I just downloaded - and it's
        even better when they are free! Below are some ebooks that we have obtained rights
        to freely distribute. Please go ahead and download them and check them out. Enjoy
        and if you have any questions or even suggestions, <a href="../../Support/Default.aspx">
            let us know</a>. You can just right click on the links below and select Save
        Target As... Don't forget to check back occasionally as we do add ebooks as we can.</p>
    <h4>
        Internet Marketing
    </h4>
    <p>
        <a href="greatestsecrets.exe">The Greatest Marketing Secrets of the Ages - by Yanik
            Silver</a></p>
    <p>
        <a href="killerads.exe">How to Write Killer Ads</a></p>
    <p>
        <a href="kiss.exe">Free Internet Stuff</a></p>
    <p>
        <a href="guru.exe">Internet Marketing Tips From the Experts</a></p>
    <p>
        <a href="magicletters.exe">Magic Letters</a> - How to write so people buy</p>
    <p>
        <a href="unlimitedprofits.exe">Unlimited Profits</a></p>
    <p>
        <a href="magnetic.exe">Magnetic Internet Power Marketing</a></p>
    <p>
        <a href="MWTips.exe">A Newbie Primer on Internet Marketing</a></p>
    <p>
        <a href="pdcb.exe">Practical .COM Businesses</a></p>
    <p>
        <a href="scientific.exe">Scientific Advertising</a></p>
    <p>
        <a href="solo_book_pp.zip">Advice on becoming a solo professional</a></p>
    <p>
        <a href="startbiz.zip">Online Business Basics</a></p>
    <p>
        <a href="stealth.exe">Stealth Marketing Techniques</a></p>
    <p>
        <a href="tdwebgold.exe">Web Gold - that might be a streach but that's the goal</a></p>
    <p>
        <a href="stealth.exe">Stealth Marketing Techniques</a></p>
    <p>
        <a href="tdTraffic.exe">How to Start Your Own Traffic Virus</a></p>
    <p>
        <a href="leskoebook.exe">How I Started My Own Internet Business</a> - remember that
        guy on TV in the question mark suit, well he wrote an ebook.</p>
    <p>
        <a href="ebookcourse.exe">How to write, create, promote and sell E-books on the Internet</a></p>
    <p>
        <a href="Ebookomatic.pdf">eBookoMatic - More on ebooks.</a></p>
    <p>
        <a href="ebstter.exe">eBook Submitter - helps you find places to sell your ebook.</a></p>
    <p>
        <a href="7Secrets.exe">7 Secrets to Unlimited Traffic</a> - it's an oldie but it
        just proves that the best traffic tactics are still viral marketing and article
        marketing.</p>
    <p>
        <a href="BeginnersGuide.zip">The Absolute Begineer's Guide to Starting A Website</a></p>
    <p>
        <a href="auctions.exe">How to increase your auction profits.</a></p>
    <asp:PlaceHolder runat="server" ID="javaScriptPlaceholder" />
</asp:Content>
