Develop environment
===================
Current IDE to develop add-in is MS Visual Studio 2008 Profesional

* `Prepare development environment`_
* `Config project`_

Prepare development environment
-------------------------------

* Install MS Office 2007.

* Install MS Visual Studio 2008 with Visual C# and Visual Studio Tools for Office.

* Download and install `Git <http://git-scm.com/download>`_

* Download and install `Git extensions <http://code.google.com/p/gitextensions>`_

* Download and install `Git source control provider <http://gitscc.codeplex.com/releases/view/46589>`_ for VS2008

* Clone develop repository (if you want to contribute to the development first make a fork of repository)

* Install web server (Apache) to distribute add-in releases.
	* Add the following lines in your apache config in mime types section
		.. code-block:: xml
			:linenos:

			AddType application/x-ms-application application
			AddType application/x-ms-manifest manifest
			AddType application/octet-stream deploy
			AddType application/vnd.ms-xpsdocument xps
			AddType application/xaml+xml xaml
			AddType application/x-ms-xbap xbap
			AddType application/x-silverlight-app xap
			AddType application/microsoftpatch msp
			AddType application/microsoftupdate msu
			AddType application/x-ms-vsto vsto

* Install FTP server to deploy add-in releases.

Config project
--------------

* Copy CLONE_DIR/RegexMarkup/mail.config_sample to CLONE_DIR/RegexMarkup/mail.config and edit values of mailSettings
* Open project in VS2008
