<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="text" omit-xml-declaration="yes" />

<xsl:template match="ProducedCars">ProductionYear,Model,VIN
<xsl:apply-templates select="Car"/>
</xsl:template>
<xsl:template match="Car">
<xsl:value-of select="ProductionYear"/>,<xsl:value-of select="Model"/>,<xsl:value-of select="VIN"/><xsl:text>&#xa;</xsl:text>
</xsl:template>
</xsl:stylesheet>
