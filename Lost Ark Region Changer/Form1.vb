Imports System.Xml
Public Class Form1
    Dim strSteamInstallPath As String = Nothing
    Dim strRegionCode As String = "CE"
    Dim strConfig As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Environment.Is64BitOperatingSystem Then

            strSteamInstallPath = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", Nothing)
        Else
            strSteamInstallPath = My.Computer.Registry.GetValue(
    "HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", Nothing)
        End If
        If IsNothing(strSteamInstallPath) Then
            MsgBox("No Steam-Client-Installation found! Exiting")
            Application.Exit()
        End If
        strConfig = strSteamInstallPath & "\steamapps\common\Lost Ark\EFGame\Config\UserOption.xml"

        Dim xml As New XmlDocument
        xml.Load(strConfig)
        If IsNothing(xml.InnerXml) Then
            MsgBox("Lost Ark Confing not found! Exiting")
            Application.Exit()
        End If
        Dim regionNode As XmlNode = xml.SelectSingleNode("UserOption/SaveAccountOptionData/RegionID")
        strRegionCode = regionNode.InnerText

        cbRegion.Items.Clear()
        cbRegion.Items.Add("Europe Central")
        cbRegion.Items.Add("Europe West")
        cbRegion.Items.Add("North America East")
        cbRegion.Items.Add("North America West")
        cbRegion.Items.Add("Southamerica")
        cbRegion.SelectedItem = getSelectedFromCode(strRegionCode)

    End Sub

    Private Function getSelectedFromCode(ByVal strCode As String)
        If strCode = "EA" Then
            Return "North America East"
        ElseIf strCode = "WA" Then
            Return "North America West"
        ElseIf strCode = "SA" Then
            Return "Southamerica"
        ElseIf strCode = "WE" Then
            Return "Europe West"
        Else
            Return "Europe Central"
        End If
    End Function

    Private Function getCodeFromSelected(ByVal strCode As String)
        If strCode = "North America East" Then
            Return "EA"
        ElseIf strCode = "North America West" Then
            Return "WA"
        ElseIf strCode = "Southamerica" Then
            Return "SA"
        ElseIf strCode = "Europe West" Then
            Return "WE"
        Else
            Return "CE"
        End If
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Process.Start("steam://rungameid/1599340")
        Dim psi As ProcessStartInfo = New ProcessStartInfo("steam://rungameid/1599340")
        psi.UseShellExecute = True
        Process.Start(psi)
        Application.Exit()
    End Sub

    Private Sub cbRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbRegion.SelectedIndexChanged
        strRegionCode = getCodeFromSelected(cbRegion.SelectedItem)
        Dim xml As New XmlDocument
        xml.Load(strConfig)
        xml.SelectSingleNode("UserOption/SaveAccountOptionData/RegionID").InnerText = strRegionCode
        Dim xw As XmlWriter = XmlWriter.Create(strConfig)
        xml.WriteTo(xw)
        xw.Close()
    End Sub
End Class
