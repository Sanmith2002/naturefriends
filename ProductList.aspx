<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="naturefriends.ProductList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Product List</title>
    <style>
        .table-container {
            max-width: 800px;
            margin: 30px auto;
            font-family: Arial;
        }
        h2 {
            text-align: center;
        }
        .action-buttons a {
            margin: 0 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="table-container">
            <h2>Product List</h2>

            <asp:GridView ID="gridProducts" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="productid" OnRowDeleting="gridProducts_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="productid" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="name" HeaderText="Product Name" />
                    <asp:BoundField DataField="description" HeaderText="Description" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div class="action-buttons">
                                <asp:HyperLink ID="lnkEdit" runat="server" 
                                    NavigateUrl='<%# "EditProduct.aspx?id=" + Eval("productid") %>' 
                                    Text="Edit" />
                                |
                                <asp:LinkButton ID="lnkDelete" runat="server" 
                                    Text="Delete" CommandName="Delete" 
                                    CommandArgument='<%# Eval("productid") %>' 
                                    OnClientClick="return confirm('Are you sure you want to delete this product?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <br />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
        </div>
    </form>
</body>
</html>
