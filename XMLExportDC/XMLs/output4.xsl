<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:csv="csv:csv">
  <xsl:output method="text" omit-xml-declaration="yes" indent="no"/>

  
  <xsl:template match="/">
    
    <xsl:for-each select="/*/*">
      <xsl:value-of select="ProductionYear"/>
      <xsl:text>,</xsl:text>
      <xsl:value-of select="Model"/>
      <xsl:text>,</xsl:text>
      <xsl:value-of select="VIN"/>
      <xsl:text>&#xa;</xsl:text>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
