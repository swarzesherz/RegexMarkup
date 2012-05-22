Rules definition file
=====================
In this section describes the xml file that contains struct with regular expression patterns and definition of these, to automatic tagging of journals.	

* `Main struct`_
* `Journal element struct`_

Main struct
-----------

The main element is ``<rules>`` that contains one or more ``<journal issn="XXXX-XXXX"/>`` elements to define rules (regular expressions patterns)

.. code-block:: xml
	:linenos:

	<?xml version="1.0" encoding="utf-8" ?>
	/*
	*<!ELEMENT rules ( journal+ ) >
	*/
	<rules>
		<journal issn="0366-2128">...</journal>
		<journal issn="1405-9940">...</journal>
		<journal issn="1026-8774">...</journal>
		...
	</rules>

Journal element struct
----------------------

.. code-block:: xml
	:linenos:

	...
	/*
	*<!ELEMENT journal ( name, norm, regex, struct ) >
	*<!ATTLIST journal issn NMTOKEN #REQUIRED >
	*/
	<journal issn="XXXX-XXXX">
		/*
		*<!ELEMENT name ( #PCDATA ) >
		*/
		<name>Journal name</name>
		/*
		*<!ELEMENT norm ( #PCDATA ) >
		*/
		<norm>other</norm>
		/*
		*<!ELEMENT regex ( #PCDATA ) >
		*/
		<regex>(...+)</regex>
		/*
		*<!ELEMENT struct ( tagname+ ) >
		*/
		<struct>
			/*
			*<!ELEMENT tagname ( attr?, prevalue?, value?, ( regex, struct  | multiple)?, tagname*, postvalue? ) >
			*<!ATTLIST tagname tag ( true ) #IMPLIED >
			*/
			<tagname tag="true">
				/*
				*<!ELEMENT attr ( attrname+ ) >
				*/
				<attr>
					/*
					*<!ELEMENT attrname ( #PCDATA ) >
					*/
					<attrname></attrname>
				</attr>

				/*
				*<!ELEMENT prevalue ( #PCDATA ) >
				*/
				<prevalue></prevalue>

				/*
				*<!ELEMENT value ( #PCDATA ) >
				*/
				<value><value>

				/*
				*<!ELEMENT postvale ( #PCDATA ) >
				*/
				<postvale></postvalue>

				/*
				*<!ELEMENT regex ( #PCDATA ) >
				*/
				<regex>(...+)</regex>

				/*

				/*
				*<!ELEMENT struct ( tagname+ ) >
				*/
				<struct>
					...
				</struct>

				*<!ELEMENT multiple ( option+ ) >
				*/
				<multiple>
					/*
					*<!ELEMENT option ( regex, struct ) >
					*/
					<option></option>
				</multiple>

			</tagname>
		</struct>
	</journal>
	...

* **journal** .- This element requires attribute issn to identify each of these in XML file.
	.. code-block:: xml
		:linenos:

		...
		<journal issn="1405-9940">
			...
		</journal>
		...

	* **name** .- The element contains the canonical name of journal
		.. code-block:: xml
			:linenos:

			...
			<name>Archivos de cardiología de México</name>
			...


	* **norm** .- In this element is specified "norm" of references in journal (other, apa, vancouv, iso690)
		.. code-block:: xml
			:linenos:

			...
			<norm>other</norm>
			...

	* **regex** .- This contains an regular expression pattern to tag reference.
		.. code-block:: xml
			:linenos:

			...
			<regex>(....+)</regex>
			...

	* **struct** .- In this node appear all tags to markup reference or group in regular expression pattern.

		* **tagname** .- This is a generic name to this element, really can take all tag names in the norm (other, apa, vancouv, iso690) or generic name if assumes this is only a container. If this tag need appear in tagging is required put attribute **tag="true"** like: 
			.. code-block:: xml
				:linenos:

				...
				<ocitat tag="true">
					...
				</ocitat>
				...

			* **attr** .- This element contains attributes of tag if require put default value in these.
				* **attrname** .- Is a generic name to element,  this take name of attributes of tag like [oauthor **role="nd"**] is represented:
					.. code-block:: xml
						:linenos:

						...
						<oauthor tag="true">
							<attr>
								<role>nd</role>
							</attr>
							...
						</oauthor>
						...
				* **prevalue** .- Contains backreference of group in regular expression pattern and print the value before tag.
				* **value** .- Contains backreference of group in regular expression pattern and print the value in middle of tag.
				* **postvalue** .- Contains backreference of group in regular expression pattern and print the value after tag.
					.. code-block:: xml
						:linenos:

						...
						/*
						* Example string and regex
						* ...; Castillo, G. F.; ...
						* (.+?;\s)(Castillo, G. F.)(;\s.+)
						* Group 1:	...;
						* Group 2:	Castillo, G. F.
						* Group 3:	; ...
						*/
						<oauthor tag="true">
							...
							<prevalue>$1</prevalue>
							<value>$2</value>
							<postvalue>$3</postvalue>
							...
						</oauthor>
						/*
						* Result
						* ...; [oauthor]Castillo, G. F.[/oauthor]; ...
						*/
				* **regex** .- This element used when value of tag contains child tags, if this tag is used you need  put **struct** element and inner tags
					.. code-block:: xml
						:linenos:

						...
						/*
						* Example string and regex
						* ...; Castillo, G. F.; ...
						* (.+?;\s)(Castillo, G. F.)(;\s.+)
						* Group 2:	Castillo, G. F.
						* Group 2 contains surname and fname and can parse with this regex
						* (Castillo)(,\s)(G\.\sF\.)
						*/
						<oauthor tag="true">
							...
							<prevalue>$1</prevalue>
							<value>$2</value>
							<regex>(Castillo)(,\s)(G\.\sF\.)</regex>
							<struct>
								<surname tag="true">
									<value>$1</value>
									<postvalue>$2</postvalue>
								</surname>
								<fname>
									<value>$3</value>
								</fname>
							</struct>
							<postvalue>$3</postvalue>
							...
						</oauthor>
						/*
						* Result
						* ...; [oauthor][surname]Castillo[/surname], [fname]G. F.[/fname][/oauthor]; ...
						*/

				* **multiple** .- In case the value element can parsed by more than one regular expression, use this element and put each pattern in sub element **option**
					* **option** .- This element contains each **regex** pattern and **struct** of these.

					.. code-block:: xml
						:linenos:

						...
						/*
						* Example strings and regex
						* ...; Castillo, G. F.; ...
						* (.+?;\s)(Castillo, G. F.)(;\s.+)
						* (.+?;\s)(G. F., Castillo)(;\s.+)
						* Group 2:	Castillo, G. F.
						* Group 2:	G. F., Castillo
						* Group 2 can parsed with these regex
						* (Castillo)(,\s)(G\.\sF\.)
						* (G\.\sF\.)(,\s)(Castillo)
						*/
						<oauthor tag="true">
							...
							<prevalue>$1</prevalue>
							<value>$2</value>
							<multiple>
								<option>
									<regex>(Castillo)(,\s)(G\.\sF\.)</regex>
									<struct>
										<surname tag="true">
											<value>$1</value>
											<postvalue>$2</postvalue>
										</surname>
										<fname>
											<value>$3</value>
										</fname>
									</struct>
								</option>
								<option>
									<regex>(G\.\sF\.)(,\s)(Castillo)</regex>
									<struct>
										<surname tag="true">
											<value>$1</value>
											<postvalue>$2</postvalue>
										</surname>
										<fname>
											<value>$3</value>
										</fname>
									</struct>
								</option>
							</multiple>
							<postvalue>$3</postvalue>
							...
						</oauthor>
						/*
						* Results
						* ...; [oauthor][surname]Castillo[/surname], [fname]G. F.[/fname][/oauthor]; ...
						* ...; [oauthor][surname]G. F.[/surname], [fname]Castillo[/fname][/oauthor]; ...
						*/
