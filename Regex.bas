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
    'Leemos y verificamos que el iss exista
    issn = getAttrValueInTag("article", "issn")
    If issn = vbNullString Then
        MsgBox "No esta definido issn"
    Else
        'MsgBox issn, vbOKOnly, "ISSN"
        'Cargamos el archivo xml donde se encuetran los patrones de las revistas
        If Not xmlDoc.Load("C:\Documents and Settings\Herz\Mis documentos\Dropbox\SciELO_Files\Automatas\regex.xml") Then
            MsgBox "No se pudo cargar el archivo xml"
        Else
            'Leemos el nodo correspondiente al issn de la revista
            Set objElem = xmlDoc.SelectSingleNode("//*[@issn=""" & issn & """]")
            If objElem Is Nothing Then
                MsgBox "No se econtro la revista en el archivo xml"
            Else
                'Asignamos los grupos en los que se divide la revista
                Set groupsXML = objElem.SelectSingleNode("grupos")
                'Pattern Search
                patternString = objElem.SelectSingleNode("regex").Text
                'MsgBox patternString, vbOKOnly, "Pattern String"
                'Verificamos que la seleccion sea del parrafo completo
                If Selection.Paragraphs.First.Range.Start <> Selection.Range.Start Or Selection.Paragraphs.Last.Range.End <> Selection.Range.End Then
                    MsgBox "Seleccione la cita(s) completamente"
                Else
                    'Asign text to subjectString
                    subjetcString = Selection.Text
                    'MsgBox subjetcString, vbOKOnly, "Texto Seleccionado"
                    'Buscando parrafo por parrafo
                    Set parrafos = Selection.Paragraphs
                    For Each parrafo In parrafos
                        'Mandamos el texto de cada parrafo a una funcion que nos lo regresara marcado y quitamos el salto linea
                        subjetcString = ActiveDocument.Range(Start:=parrafo.Range.Start, End:=(parrafo.Range.End - 1)).Text
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
   Dim backreferencePostValue As String
   Dim backreferencePostValueString As String
   Dim backreferencePreValue As String
   Dim backreferencePreValueString As String
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
    'Iteramos los nodos dentro del xml que nos dan el contenido de las citas
      For Each itemXML In groups.ChildNodes
        'Verificamos si el nodo es una etiqueta(tag) o no
        If Not IsNull(itemXML.getAttribute("notag")) Then
            tagStringOpen = vbNullString
            tagStringClose = vbNullString
        Else
            tagStringOpen = "[" & itemXML.nodeName & "]"
            tagStringClose = "[/" & itemXML.nodeName & "]"
        End If
        'Verificamos si el nodo contiene un valor directo para la etiqueta(tag) o si su valor esta compuesto otras etiquetas(tag)
        If (itemXML.SelectSingleNode("value") Is Nothing) Then
            'Si esta compuesto de otras etiquetas(tag) volvemos a enviar la cadena y el patron con los nodos hijos de la etiqueta(tag)
            RetStr = RetStr & tagStringOpen & TestRegExp(myPattern, myString, itemXML) & tagStringClose
        Else
            'Deacuedo al valor que tenemos indicado en el xml extramos la cadena de texto correspondiente
            backreference = itemXML.SelectSingleNode("value").Text
            subjetcString = objRegExp.Replace(objMatch.Value, backreference)
            'Verificamos si a la cadena de texto resultante hay que aplicarle un patron nuevo y si no agregamos las etiquetas(tag) directamente
            If Not (itemXML.SelectSingleNode("regex") Is Nothing) Then
                'Verificamos si hay que poner un valor antes de la etiqueta(tag)
                If Not (itemXML.SelectSingleNode("prevalue") Is Nothing) Then
                    backreferencePreValue = itemXML.SelectSingleNode("prevalue").Text
                    backreferencePreValueString = objRegExp.Replace(objMatch.Value, backreferencePreValue)
                    RetStr = RetStr & backreferencePreValueString
                End If
                Set groupsXML = itemXML.SelectSingleNode("grupos")
                patternString = itemXML.SelectSingleNode("regex").Text
                'Enviamos la cadena resultante con el patron nuevo a aplicar y las etiquetas(tag) que debe contener y el resultado lo ponemos dentro de la etiqueta(tag) correspondiente
                RetStr = RetStr & tagStringOpen & TestRegExp(patternString, subjetcString, groupsXML) & tagStringClose
                'Verificamos si hay que poner un valor despues de la etiqueta(tag)
                If Not (itemXML.SelectSingleNode("postvalue") Is Nothing) Then
                    backreferencePostValue = itemXML.SelectSingleNode("postvalue").Text
                    backreferencePostValueString = objRegExp.Replace(objMatch.Value, backreferencePostValue)
                    RetStr = RetStr & backreferencePostValueString
                End If
            Else
                replaceString = vbNullString
                'Verificamos si hay que poner un valor antes de la etiqueta(tag)
                If Not (itemXML.SelectSingleNode("prevalue") Is Nothing) Then
                    backreferencePreValue = itemXML.SelectSingleNode("prevalue").Text
                    replaceString = replaceString & backreferencePreValue
                End If
                'Armamos la etiqueta(tag) con su valor
                replaceString = replaceString & tagStringOpen & backreference & tagStringClose
                'Verificamos si hay que poner un valor antes de la etiqueta(tag)
                If Not (itemXML.SelectSingleNode("postvalue") Is Nothing) Then
                    backreferencePostValue = itemXML.SelectSingleNode("postvalue").Text
                    replaceString = replaceString & backreferencePostValue
                End If
                RetStr = RetStr & objRegExp.Replace(myString, replaceString)
            End If
        End If
      Next
    Next
    
   Else
    RetStr = RetStr & myString
   End If
   TestRegExp = RetStr
End Function