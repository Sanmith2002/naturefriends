<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="naturefriends.ProductList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Naturefriends Dashboard</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="d-flex justify-content-between align-items-center mb-3">
                <h2>Naturefriends Product Dashboard</h2>
                <asp:HyperLink ID="hlAddProduct" runat="server" CssClass="btn btn-primary" NavigateUrl="AddProduct.aspx">
                    Add Product
                </asp:HyperLink>
            </div>

            <asp:GridView ID="gridProducts" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered"
                DataKeyNames="productid" OnRowDeleting="gridProducts_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="productid" HeaderText="ID" ReadOnly="True" ItemStyle-Width="5%" />
                    <asp:BoundField DataField="name" HeaderText="Product Name" ItemStyle-Width="20%" />
                    <asp:BoundField DataField="description" HeaderText="Description" ItemStyle-Width="25%" />

                    <asp:BoundField DataField="cutting_stock" HeaderText="Cutting Stock" ItemStyle-Width="7%" />
                    <asp:BoundField DataField="skin_stock" HeaderText="Skin Stock" ItemStyle-Width="7%" />
                    <asp:BoundField DataField="finishing_stock" HeaderText="Finishing Stock" ItemStyle-Width="7%" />

                    <asp:BoundField DataField="manufacturing_cost" HeaderText="Manufacturing Cost" DataFormatString="{0:C}" ItemStyle-Width="10%" />
                    <asp:BoundField DataField="selling_price" HeaderText="Selling Price" DataFormatString="{0:C}" ItemStyle-Width="10%" />

                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="14%">
                        <ItemTemplate>
                            <div class="d-flex flex-nowrap">
                                <asp:HyperLink ID="lnkEdit" runat="server" 
                                    NavigateUrl='<%# "EditProduct.aspx?id=" + Eval("productid") %>' 
                                    CssClass="btn btn-sm btn-warning me-2">
                                    Edit
                                </asp:HyperLink>

                                <asp:LinkButton ID="lnkDelete" runat="server" 
                                    Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("productid") %>' 
                                    CssClass="btn btn-sm btn-danger"
                                    OnClientClick="return confirm('Are you sure you want to delete this product?');" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

            <asp:Label ID="lblMessage" runat="server" ForeColor="Green" CssClass="mt-3 d-block" />
        </div>

        <!-- Bootstrap JS Bundle -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
