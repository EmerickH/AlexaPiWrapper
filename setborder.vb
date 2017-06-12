Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Module setborder
    'See: System.Windows.Forms.SafeNativeMethods.SetWindowPos
    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
    Private Function SetWindowPos(ByVal hWnd As HandleRef, ByVal hWndInsertAfter As HandleRef, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal flags As Integer) As Boolean
    End Function

    'See: System.Windows.Forms.UnSafeNativeMethods.GetWindowLong*
    <DllImport("user32.dll", EntryPoint:="GetWindowLong", CharSet:=CharSet.Auto)>
    Private Function GetWindowLong32(ByVal hWnd As HandleRef, ByVal nIndex As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="GetWindowLongPtr", CharSet:=CharSet.Auto)>
    Private Function GetWindowLongPtr64(ByVal hWnd As HandleRef, ByVal nIndex As Integer) As IntPtr
    End Function

    Private Function GetWindowLong(ByVal hWnd As HandleRef, ByVal nIndex As Integer) As IntPtr
        If (IntPtr.Size = 4) Then
            Return GetWindowLong32(hWnd, nIndex)
        End If
        Return GetWindowLongPtr64(hWnd, nIndex)
    End Function

    'See: System.Windows.Forms.UnSafeNativeMethods.SetWindowLong*
    <DllImport("user32.dll", EntryPoint:="SetWindowLong", CharSet:=CharSet.Auto)>
    Private Function SetWindowLongPtr32(ByVal hWnd As HandleRef, ByVal nIndex As Integer, ByVal dwNewLong As HandleRef) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowLongPtr", CharSet:=CharSet.Auto)>
    Private Function SetWindowLongPtr64(ByVal hWnd As HandleRef, ByVal nIndex As Integer, ByVal dwNewLong As HandleRef) As IntPtr
    End Function

    Public Function SetWindowLong(ByVal hWnd As HandleRef, ByVal nIndex As Integer, ByVal dwNewLong As HandleRef) As IntPtr
        If (IntPtr.Size = 4) Then
            Return SetWindowLongPtr32(hWnd, nIndex, dwNewLong)
        End If
        Return SetWindowLongPtr64(hWnd, nIndex, dwNewLong)
    End Function

    'See: System.Windows.Forms.Control.SetWindowStyle
    Public Sub SetWindowStyle(ByVal handle As IntPtr, ByVal flag As Integer, ByVal value As Boolean)
        Dim windowLong As Integer = CInt(CLng(GetWindowLong(New HandleRef(Nothing, handle), -16)))
        Dim ip As IntPtr
        If value Then
            ip = New IntPtr(windowLong Or flag)
        Else
            ip = New IntPtr(windowLong And Not flag)
        End If
        SetWindowLong(New HandleRef(Nothing, handle), -16, New HandleRef(Nothing, ip))
    End Sub

    <Extension()>
    Public Sub ShowCaption(ByVal form As IntPtr)
        SetWindowStyle(form, &H400000, True)
        ApplyStyleChanges(form)
    End Sub

    <Extension()>
    Public Sub HideCaption(ByVal form As IntPtr)
        SetWindowStyle(form, &H400000, False)
        ApplyStyleChanges(form)
    End Sub

    Public NullHandleRef As HandleRef
    <Extension()>
    Public Function ApplyStyleChanges(ByVal form As IntPtr) As Boolean
        Return SetWindowPos(New HandleRef(Nothing, form), NullHandleRef, 0, 0, 0, 0, &H37)
    End Function
End Module
