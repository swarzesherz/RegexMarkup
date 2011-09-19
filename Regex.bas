Attribute VB_Name = "Regex"
Sub regex()
    'Variable declaration
    Dim patternString As String
    Dim subjetcString As String
    Dim parrafos As Paragraphs
    Dim parrafo As Paragraph
    Dim replaceText As String
    Dim xmlDoc As DOMDocument
    Dim issn As String
    Dim objElem As IXMLDOMElement
    Dim groupsXML As IXMLDOMElement
    'Variable initialitation
    Set xmlDoc = New DOMDocument
    issn = getAttrValueInTag("article", "issn")
    If issn = vbNullString Then
        MsgBox "No esta definido issn"
    Else
        MsgBox issn, vbOKOnly, "ISSN"
        If Not xmlDoc.Load("C:\Documents and Settings\Herz\Mis documentos\Dropbox\SciELO_Files\Automatas\regex.xml") Then
            MsgBox "No se pudo cargar el archivo xml"
        Else
            Set objElem = xmlDoc.SelectSingleNode("//*[@issn=""" & issn & """]")
            If objElem Is Nothing Then
                MsgBox "No se econtro la revista en el archivo xml"
            Else
                Set groupsXML = objElem.SelectSingleNode("grupos")

                'Pattern Search
                patternString = objElem.SelectSingleNode("regex").Text
                MsgBox patternString, vbOKOnly, "Pattern String"
                If Selection.Paragraphs.First.Range.Start <> Selection.Range.Start Or Selection.Paragraphs.Last.Range.End <> Selection.Range.End Then
                    MsgBox "Seleccione la cita(s) completamente"
                Else
                    'Asign text to subjectString
                    subjetcString = Selection.Text
                    MsgBox subjetcString, vbOKOnly, "Texto Seleccionado"
                    'Buscando parrafo por parrafo
                    Set parrafos = Selection.Paragraphs
                    For Each parrafo In parrafos
                        subjetcString = parrafo.Range.Text
                        'MsgBox (TestRegExp(patternString, subjetcString))
                        replaceText = replaceText & TestRegExp(patternString, subjetcString, groupsXML)
                        replaceText = replaceText & vbCrLf
                    Next
                    Selection.Range.Text = replaceText
                End If
            End If
        End If
    End If
    Exit Sub
End Sub


Function getAttrValueInTag(tag As String, attr As String) As String
    'Variable declaration
    Dim objRegExp As regExp
    Dim objMatch As Match
    Dim colMatches As MatchCollection
    Dim result As String
    Dim subjectString As String
    
    
    'Variable initialitation
    Set objRegExp = New regExp
    subjectString = Trim$(ActiveDocument.Range.Text)
    
    'Pattern to get value attribute
    objRegExp.Pattern = "\[" & tag & ".*?" & attr & "=""(.*?)"".*?\]"
    
    'Set Case Insensitivity.
    objRegExp.IgnoreCase = True
   
    'Test whether the String can be compared.
    If (objRegExp.Test(subjectString) = True) Then
    'Get the matches.
        Set colMatches = objRegExp.Execute(subjectString)   ' Execute search.
        Set objMatch = colMatches(0)
        result = objMatch.SubMatches(0)
    Else
        result = vbNullString
    End If
    getAttrValueInTag = result
End Function
Function TestRegExp(myPattern As String, myString As String, groups As IXMLDOMElement)
   'Create objects.
   Dim objRegExp As regExp
   Dim objMatch As Match
   Dim colMatches   As MatchCollection
   Dim RetStr As String
   Dim itemXML As IXMLDOMElement
   Dim itemXML2 As IXMLDOMElement
   Dim groupsXML As IXMLDOMElement
   Dim patternString As String
   Dim subjetcString As String
   Dim tagStringOpen As String
   Dim tagStringClose As String
   Dim backreference As String
   Dim backreferenceSeparator As String
   Dim replaceString As String
   ' Create a regular expression object.
   Set objRegExp = New regExp

   'Set the pattern by using the Pattern property.
   objRegExp.Pattern = myPattern

   ' Set Case Insensitivity.
   objRegExp.IgnoreCase = True

   'Set global applicability.
   objRegExp.Global = True
   
   'Test whether the String can be compared.
   If (objRegExp.Test(myString) = True) Then

   'Get the matches.
    Set colMatches = objRegExp.Execute(myString)   ' Execute search.

    For Each objMatch In colMatches   ' Iterate Matches collection.
      'RetStr = RetStr & "Coincidencia en la posicion "
      'RetStr = RetStr & objMatch.FirstIndex & ". Coincidencia completa:"
      'RetStr = RetStr & vbCrLf
      'RetStr = RetStr & objMatch.Value
      For Each itemXML In groups.ChildNodes
        If Not (itemXML.SelectSingleNode("value") Is Nothing) Then
            backreference = itemXML.SelectSingleNode("value").Text
            subjetcString = objMatch.SubMatches((CInt(backreference) - 1))
            If Not IsNull(itemXML.getAttribute("notag")) Then
                tagStringOpen = vbNullString
                tagStringClose = vbNullString
            Else
                tagStringOpen = "[" & itemXML.nodeName & "]"
                tagStringClose = "[/" & itemXML.nodeName & "]"
            End If
            'RetStr = RetStr & " SubGrupo " & itemXML.nodeName & ": "
            'RetStr = RetStr & subjetcString
            'RetStr = RetStr & vbCrLf
            If Not (itemXML.SelectSingleNode("regex") Is Nothing) Then
                Set groupsXML = itemXML.SelectSingleNode("grupos")
                patternString = itemXML.SelectSingleNode("regex").Text
                'RetStr = RetStr & TestRegExp(patternString, subjetcString, groupsXML)
                RetStr = RetStr & tagStringOpen & TestRegExp(patternString, subjetcString, groupsXML) & tagStringClose
            Else
                If Not (itemXML.SelectSingleNode("separator") Is Nothing) Then
                    backreferenceSeparator = itemXML.SelectSingleNode("separator").Text
                    replaceString = tagStringOpen & "$" & backreference & tagStringClose & "$" & backreferenceSeparator
                Else
                    replaceString = tagStringOpen & "$" & backreference & tagStringClose
                End If
                RetStr = RetStr & objRegExp.Replace(myString, replaceString)
                'MsgBox replaceString, vbOKOnly, "replace string"
                'RetStr = RetStr & "[" & tagString & "]" & subjetcString & "[/" & tagString & "]"
            End If
        Else
            RetStr = RetStr & "[" & itemXML.nodeName & "]"
            For Each itemXML2 In itemXML.ChildNodes
                backreference = itemXML2.SelectSingleNode("value").Text
                subjetcString = objMatch.SubMatches((CInt(backreference) - 1))
                If Not IsNull(itemXML2.getAttribute("notag")) Then
                    tagStringOpen = vbNullString
                    tagStringClose = vbNullString
                Else
                    tagStringOpen = "[" & itemXML2.nodeName & "]"
                    tagStringClose = "[/" & itemXML2.nodeName & "]"
                End If
                'RetStr = RetStr & " SubGrupo " & itemXML.nodeName & ": "
                'RetStr = RetStr & subjetcString
                'RetStr = RetStr & vbCrLf
                If Not (itemXML2.SelectSingleNode("regex") Is Nothing) Then
                    Set groupsXML = itemXML2.SelectSingleNode("grupos")
                    patternString = itemXML2.SelectSingleNode("regex").Text
                    'RetStr = RetStr & TestRegExp(patternString, subjetcString, groupsXML)
                    RetStr = RetStr & tagStringOpen & TestRegExp(patternString, subjetcString, groupsXML) & tagStringClose
                Else
                    If Not (itemXML2.SelectSingleNode("separator") Is Nothing) Then
                        backreferenceSeparator = itemXML2.SelectSingleNode("separator").Text
                        replaceString = tagStringOpen & "$" & backreference & tagStringClose & "$" & backreferenceSeparator
                    Else
                        replaceString = tagStringOpen & "$" & backreference & tagStringClose
                    End If
                    RetStr = RetStr & objRegExp.Replace(myString, replaceString)
                    'MsgBox replaceString, vbOKOnly, "replace string"
                    'RetStr = RetStr & "[" & tagString & "]" & subjetcString & "[/" & tagString & "]"
                End If
            Next
            RetStr = RetStr & "[/" & itemXML.nodeName & "]"
        End If
      Next
    Next
    
   Else
    RetStr = "No hay coincidencias en la cadena:" & vbCrLf
    RetStr = RetStr & myString & vbCrLf
   End If
   TestRegExp = RetStr
End Function
