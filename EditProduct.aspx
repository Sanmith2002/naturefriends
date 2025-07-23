<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProduct.aspx.cs" Inherits="naturefriends.EditProduct" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Product - Naturefriends</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="container mt-5" style="max-width:600px;">
        <h2 class="mb-4">Edit Product</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="mb-3 d-block" />

        <asp:HiddenField ID="hfProductId" runat="server" />

        <div class="mb-3">
            <label for="txtName" class="form-label">Product Name <span class="text-danger">*</span></label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>

        <div class="mb-3">
            <label for="txtDescription" class="form-label">Description</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
        </div>

        <div class="row">
            <div class="col-md-4 mb-3">
                <label for="txtCuttingStock" class="form-label">Cutting Stock</label>
                <asp:TextBox ID="txtCuttingStock" runat="server" CssClass="form-control" TextMode="Number" />
            </div>
            <div class="col-md-4 mb-3">
                <label for="txtSkinStock" class="form-label">Skin Stock</label>
                <asp:TextBox ID="txtSkinStock" runat="server" CssClass="form-control" TextMode="Number" />
            </div>
            <div class="col-md-4 mb-3">
                <label for="txtFinishingStock" class="form-label">Finishing Stock</label>
                <asp:TextBox ID="txtFinishingStock" runat="server" CssClass="form-control" TextMode="Number" />
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 mb-3">
                <label for="txtManufacturingCost" class="form-label">Manufacturing Cost</label>
                <asp:TextBox ID="txtManufacturingCost" runat="server" CssClass="form-control" TextMode="Number" Step="0.01" />
            </div>
            <div class="col-md-6 mb-3">
                <label for="txtSellingPrice" class="form-label">Selling Price</label>
                <asp:TextBox ID="txtSellingPrice" runat="server" CssClass="form-control" TextMode="Number" Step="0.01" />
            </div>
        </div>

        <asp:Button ID="btnUpdateProduct" runat="server" CssClass="btn btn-primary" Text="Update Product" OnClick="btnUpdateProduct_Click" />

        <!-- Bootstrap JS Bundle -->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
