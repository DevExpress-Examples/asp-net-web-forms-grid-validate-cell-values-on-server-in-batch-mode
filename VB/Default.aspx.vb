﻿Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Linq
Imports DevExpress.Web.Data
Imports DevExpress.Web
Imports System.Threading

Partial Public Class _Default
	Inherits System.Web.UI.Page

	Protected ReadOnly Property GridData() As List(Of GridDataItem)
		Get
			Dim key = "34FAA431-CF79-4869-9488-93F6AAE81263"
			If Not IsPostBack OrElse Session(key) Is Nothing Then
				Session(key) = Enumerable.Range(0, 100).Select(Function(i) New GridDataItem With {
					.ID = i,
					.C1 = i Mod 2,
					.C2 = i * 0.5 Mod 3,
					.C3 = "C3 " & i,
					.C4 = i Mod 2 = 0,
					.C5 = "C5 " & i
				}).ToList()
			End If
			Return CType(Session(key), List(Of GridDataItem))
		End Get
	End Property
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		Grid.DataSource = GridData
		Grid.DataBind()
	End Sub
	Protected Sub Grid_RowInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
		InsertNewItem(e.NewValues)
		CancelEditing(e)
	End Sub
	Protected Sub Grid_RowUpdating(ByVal sender As Object, ByVal e As ASPxDataUpdatingEventArgs)
		UpdateItem(e.Keys, e.NewValues)
		CancelEditing(e)
	End Sub
	Protected Sub Grid_RowDeleting(ByVal sender As Object, ByVal e As ASPxDataDeletingEventArgs)
		DeleteItem(e.Keys, e.Values)
		CancelEditing(e)
	End Sub
	Protected Sub Grid_BatchUpdate(ByVal sender As Object, ByVal e As ASPxDataBatchUpdateEventArgs)
		For Each args In e.InsertValues
			InsertNewItem(args.NewValues)
		Next args
		For Each args In e.UpdateValues
			UpdateItem(args.Keys, args.NewValues)
		Next args
		For Each args In e.DeleteValues
			DeleteItem(args.Keys, args.Values)
		Next args

		e.Handled = True
	End Sub
	Protected Function InsertNewItem(ByVal newValues As OrderedDictionary) As GridDataItem
		Dim item = New GridDataItem() With {.ID = GridData.Count}
		LoadNewValues(item, newValues)
		GridData.Add(item)
		Return item
	End Function
	Protected Function UpdateItem(ByVal keys As OrderedDictionary, ByVal newValues As OrderedDictionary) As GridDataItem
		Dim id = Convert.ToInt32(keys("ID"))
		Dim item = GridData.First(Function(i) i.ID = id)
		LoadNewValues(item, newValues)
		Return item
	End Function
	Protected Function DeleteItem(ByVal keys As OrderedDictionary, ByVal values As OrderedDictionary) As GridDataItem
		Dim id = Convert.ToInt32(keys("ID"))
		Dim item = GridData.First(Function(i) i.ID = id)
		GridData.Remove(item)
		Return item
	End Function
	Protected Sub LoadNewValues(ByVal item As GridDataItem, ByVal values As OrderedDictionary)
		item.C1 = Convert.ToInt32(values("C1"))
		item.C2 = Convert.ToDouble(values("C2"))
		item.C3 = Convert.ToString(values("C3"))
		item.C4 = Convert.ToBoolean(values("C4"))
		item.C5 = Convert.ToString(values("C5"))
	End Sub
	Protected Sub CancelEditing(ByVal e As CancelEventArgs)
		e.Cancel = True
		Grid.CancelEdit()
	End Sub
	Public Class GridDataItem
		Public Property ID() As Integer
		Public Property C1() As Integer
		Public Property C2() As Double
		Public Property C3() As String
		Public Property C4() As Boolean
		Public Property C5() As String
		Public Property C9() As String
	End Class
	Protected Sub ASPxCallback1_Callback(ByVal source As Object, ByVal e As DevExpress.Web.CallbackEventArgs)
		Dim parameters() As String = e.Parameter.Split("|"c)
		Select Case parameters(0)
			Case "C3"
				If parameters(1) = "AAA" Then
					e.Result = String.Format("'{0}' is invalid value", parameters(1))
				End If
			Case "C5"
				If parameters(1) = "BBB" Then
					e.Result = String.Format("'{0}' is invalid value", parameters(1))
				End If
			Case Else
		End Select
		Thread.Sleep(1000)
	End Sub
	Protected Sub Grid_CellEditorInitialize(ByVal sender As Object, ByVal e As ASPxGridViewEditorEventArgs)
		Dim editor As ASPxEdit = CType(e.Editor, ASPxEdit)
		editor.ValidationSettings.Display = Display.Dynamic
	End Sub
End Class