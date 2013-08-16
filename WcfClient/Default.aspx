<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WcfClient.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Enter integer:
            <asp:TextBox ID="txtData" runat="server" Text="7"></asp:TextBox>
            <asp:Button ID="btnSubmit" Text="Submit" runat="server" OnClick="btnSubmit_Click"/>
            <br />
            <br />
            <asp:Literal ID="ltlData" runat="server"></asp:Literal>
        </div>
    </form>
</body>
</html>
