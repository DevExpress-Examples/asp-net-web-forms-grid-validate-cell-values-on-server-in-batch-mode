<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        var fieldName;
        var currentIndex;
        var keyCode;

        function OnBatchEditStartEditing(s, e) {
            if (keyCode == 9) {
                keyCode = null;
                s.batchEditApi.StartEdit(currentIndex, grid.GetColumnByField(fieldName).index);
                e.cancel = true;
            }
            else {
                if (!isError) {
                    fieldName = e.focusedColumn.fieldName;
                    currentIndex = e.visibleIndex;
                }
                else {
                    if (fieldName != e.focusedColumn.fieldName && currentIndex != e.visibleIndex)
                        e.cancel = true;
                }
            }
        }

        var editor;
        function OnTextChanged(s, e) {
            editor = s;
            clb.PerformCallback(fieldName + '|' + s.GetText());
            lp.Show();
        }
        var isError = false;
        function OnCallbackComplete(s, e) {
            lp.Hide();

            if (e.result != null) {
                grid.batchEditApi.StartEdit(currentIndex, grid.GetColumnByField(fieldName).index);
                editor.SetIsValid(false);
                editor.SetErrorText(e.result);
                isError = true;
            }
            else
                isError = false;
        }

        function OnBatchEditEndEditing(s, e) {
            e.cancel = (isError || clb.InCallback());
        }
        function OnKeyDown(s, e) {
            keyCode = ASPxClientUtils.GetKeyCode(e.htmlEvent);
        }

    </script>
</head>
<body>
    <form id="frmMain" runat="server">
        <dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" ClientInstanceName="lp" Modal="true" Text="Checking value on the server side..."></dx:ASPxLoadingPanel>

        <dx:ASPxGridView ID="Grid" ClientInstanceName="grid" runat="server" KeyFieldName="ID" OnBatchUpdate="Grid_BatchUpdate"
            OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating" OnRowDeleting="Grid_RowDeleting" OnCellEditorInitialize="Grid_CellEditorInitialize">
            <ClientSideEvents BatchEditStartEditing="OnBatchEditStartEditing" BatchEditEndEditing="OnBatchEditEndEditing" />
            <Columns>
                <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true" />
                <dx:GridViewDataColumn FieldName="C1" />
                <dx:GridViewDataSpinEditColumn FieldName="C2" />
                <dx:GridViewDataTextColumn FieldName="C3">
                    <PropertiesTextEdit>
                        <ClientSideEvents TextChanged="OnTextChanged" KeyDown="OnKeyDown" />
                        <ValidationSettings CausesValidation="true" EnableCustomValidation="true"></ValidationSettings>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataCheckColumn FieldName="C4" />
                <dx:GridViewDataTextColumn FieldName="C5">
                    <PropertiesTextEdit>
                        <ClientSideEvents TextChanged="OnTextChanged" KeyDown="OnKeyDown" />
                        <ValidationSettings CausesValidation="true" EnableCustomValidation="true"></ValidationSettings>
                    </PropertiesTextEdit>
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsEditing Mode="Batch" />
        </dx:ASPxGridView>
        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="clb" OnCallback="ASPxCallback1_Callback">
            <ClientSideEvents CallbackComplete="OnCallbackComplete" />
        </dx:ASPxCallback>
    </form>
</body>
</html>