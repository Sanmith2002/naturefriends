<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageCutting.aspx.cs" Inherits="naturefriends.manageCutting" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Cutting Stock</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">

            <h2>Manage Cutting Stock</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="mb-3" />

            <asp:GridView ID="gridCuttingStock" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
                DataKeyNames="productid" OnRowCommand="gridCuttingStock_RowCommand">
                <Columns>
                    <asp:BoundField DataField="productid" HeaderText="ID" ReadOnly="True" ItemStyle-Width="5%" />
                    <asp:BoundField DataField="name" HeaderText="Product Name" ItemStyle-Width="40%" />
                    <asp:BoundField DataField="cutting_stock" HeaderText="Cutting Stock" ItemStyle-Width="15%" />

                    <asp:TemplateField HeaderText="Update Stock" ItemStyle-Width="30%">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAdjust" runat="server" CssClass="form-control form-control-sm" Width="80px" Text="0" />
                            <asp:Button ID="btnIncrease" runat="server" CommandName="Increase" CommandArgument='<%# Eval("productid") %>'
                                Text="Increase" CssClass="btn btn-sm btn-success mt-1 me-1" />
                            <asp:Button ID="btnDecrease" runat="server" CommandName="Decrease" CommandArgument='<%# Eval("productid") %>'
                                Text="Damage" CssClass="btn btn-sm btn-danger mt-1" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

            <asp:HyperLink ID="hlBack" runat="server" NavigateUrl="ProductList.aspx" CssClass="btn btn-secondary mt-3">
                Back to Product Dashboard
            </asp:HyperLink>

        </div>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
