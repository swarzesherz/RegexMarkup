Use main app
============

* `Before begin`_
* `Automatic tagging tool RegexMarkup`_
	* `Validating and tagging references`_
		* :ref:`Toolbar <label-toolbar>`
		* :ref:`Tagging tools <label-tagging-tools>`
		* :ref:`Navigation bar <label-navigation-bar>`

Before begin
------------

Do you need satisfy following requirements:

* One document in format HTML that contains an article or text
* The document need be previously tagged with markups of SciELO. Specifically this:
	* Article or text tag, with this attributes defined: dtd="4.0" and issn="XXXX-XXXX" like this.
		.. image:: ./images/articletag.jpg
	* Back tag that contains references, Note: The last reference require a break line at end
		.. image:: ./images/referencebr.jpg

Automatic tagging tool *RegexMarkup*
------------------------------------
* Open MS Office Word (2007 - 2010) or the Markup tool of SciELO linked to MS Office Word (2007 - 2010)
* The tools is placed in Add-ins tab

	.. image:: ./images/addins_tab.jpg

	.. image:: ./images/addins_tab_markup.jpg
* Select one o more references **"complete"**:

	**Wrong**

	.. image:: ./images/wrongselect.jpg

	**Right**

	.. image:: ./images/rightselect.jpg
* Click *RegexMarkup* button
	.. image:: ./images/regexmarkup_button.jpg
* Next window appear and you can verify and correct tags in reference(s)
	.. image:: ./images/regexmarkup_validate_window.jpg

Validating and tagging references
---------------------------------
In this windows you can verify the automatic tagging of reference, complete the tagging, ignore the tagging.

.. _label-toolbar:

**Toolbar**:

* **Save**, this button apply the changes on references (tagging) and save the document
	.. image:: ./images/regexmarkup_validate_save.jpg

* **Cancel**, this button ignore all tagging in all references
	.. image:: ./images/regexmarkup_validate_cancel.jpg

* **Delete tag**, this button delete tags hierarchically with previous selection of tag
	
	Before click button
	
	.. image:: ./images/regexmarkup_validate_before_delete_tag.jpg

	After click button

	.. image:: ./images/regexmarkup_validate_after_delete_tag.jpg

* **Edit attribute**, this button edit attributes of tags, previously selected

	.. image:: ./images/regexmarkup_validate_editattr.jpg

	When click, the next windows appear and you can modify attributes of tag

	.. image:: ./images/edit_attr_window.jpg

	1. **Edit**, this button apply changes in tag
	2. **Cancel**, this button ignore changes
	3. In this section appear attributes of tag

* **Undo**, this button undo changes
	.. image:: ./images/regexmarkup_validate_undo.jpg

* **Redo**, this button redo changes
	.. image:: ./images/regexmarkup_validate_redo.jpg

* In this section of choose **Yes** the changes of current citation is acepted, if choose **No** ignore changes. Note: All changes apply and save on click **Save** button
	.. image:: ./images/regexmarkup_validate_ignore.jpg

.. _label-tagging-tools:

**Tagging tools**

This is a hierarchical group of buttons which may tagging the reference(s), is generated according to: type of document, dtd version and specified norm on rules definition file.

* Taggin group buttons
	.. image:: ./images/tagging_tools.jpg

* **Down arrow**, when click this button you navigate to down hierarchical level

	Before perform click

	.. image:: ./images/tagging_tools_down_before.jpg

	After perfom click

	.. image:: ./images/tagging_tools_down_after.jpg

* **Up arrow**, when click this button you navigate to up hierarchical level

	Before perform click

	.. image:: ./images/tagging_tools_up_before.jpg

	After perfom click

	.. image:: ./images/tagging_tools.jpg

* **Button tag**, the button tag is te name of tag, when perform click, the current tag is added on previous selected text.

	Before click tag button

	.. image:: ./images/before_tag.jpg

	After click tag button

	.. image:: ./images/after_tag.jpg

.. _label-navigation-bar:

**Navigation bar**

When you select more than one reference, you can navigate with this bar pressing the arrows:

	.. image:: ./images/navigation_bar.jpg