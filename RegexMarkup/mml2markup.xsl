<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet 
    version="2.0" 
    xmlns:xlink="http://www.w3.org/1999/xlink" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:util="http://dtd.nlm.nih.gov/xsl/util" 
    xmlns:mml="http://www.w3.org/1998/Math/MathML"
    exclude-result-prefixes="util xsl xlink">
    <xsl:output encoding="utf-8" method="text" indent="no"/>
    
    <xsl:template name="string-replace-all">
        <xsl:param name="text" />
        <xsl:param name="replace" />
        <xsl:param name="by" />
        <xsl:choose>
            <xsl:when test="contains($text, $replace)">
                <xsl:value-of select="substring-before($text,$replace)" />
                <xsl:value-of select="$by" />
                <xsl:call-template name="string-replace-all">
                    <xsl:with-param name="text"
                        select="substring-after($text,$replace)" />
                    <xsl:with-param name="replace" select="$replace" />
                    <xsl:with-param name="by" select="$by" />
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$text" />
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <xsl:template match="*">
        <xsl:apply-templates select="." mode="format"/>
    </xsl:template>
    
    <xsl:template match="*" mode="format">
        <xsl:apply-templates select="." mode="markup"/>
    </xsl:template>
    
    <xsl:template match="*" mode="markup">
        <xsl:apply-templates select="." mode="open-tag"/>
        <xsl:apply-templates mode="format"/>
        <xsl:apply-templates select="." mode="close-tag"/>
    </xsl:template>
    <xsl:template match="*" mode="open-tag">
        <xsl:text>[</xsl:text>
        <xsl:value-of select ="name()"/>
        <xsl:apply-templates select="@*"/>
        <xsl:text>]</xsl:text>
    </xsl:template>
    <xsl:template match="*" mode="close-tag">
        <xsl:text>[/</xsl:text>
        <xsl:value-of select ="name()"/>
        <xsl:text>]</xsl:text>
    </xsl:template>
    
    <xsl:template match="@*" >
        <xsl:text> </xsl:text>
        <xsl:value-of select="name()"/>
        <xsl:text>="</xsl:text>
        <xsl:variable name="open">
            <xsl:call-template name="string-replace-all">
                <xsl:with-param name="text" select="."/>
                <xsl:with-param name="replace" select="'['" />
                <xsl:with-param name="by" select="'&amp;#91;'" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="close">
            <xsl:call-template name="string-replace-all">
                <xsl:with-param name="text" select="$open" />
                <xsl:with-param name="replace" select="']'" />
                <xsl:with-param name="by" select="'&amp;#93;'" />
            </xsl:call-template>
        </xsl:variable>
        <xsl:value-of select="$close"/>
        <xsl:text>"</xsl:text>
    </xsl:template>
    
    <xsl:template match="text()" mode="format">
        <xsl:choose>
            <xsl:when test="normalize-space(.)=''"/>
            <xsl:otherwise>
                <xsl:variable name="open">
                    <xsl:call-template name="string-replace-all">
                        <xsl:with-param name="text" select="."/>
                        <xsl:with-param name="replace" select="'['" />
                        <xsl:with-param name="by" select="'&amp;#91;'" />
                    </xsl:call-template>
                </xsl:variable>
                <xsl:variable name="close">
                    <xsl:call-template name="string-replace-all">
                        <xsl:with-param name="text" select="$open" />
                        <xsl:with-param name="replace" select="']'" />
                        <xsl:with-param name="by" select="'&amp;#93;'" />
                    </xsl:call-template>
                </xsl:variable>
                <xsl:value-of select="$close"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>