<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="//MediaTypes">
    <td>
      <xsl:for-each select="*[.='true']">
        <xsl:value-of select="name()"/>
        <xsl:if test="position() != last()">, </xsl:if>
      </xsl:for-each>
      <xsl:if test="CustomMediaType != ''">
        <xsl:if test="*[.='true']">, </xsl:if>
        <xsl:value-of select="CustomMediaType"/>
      </xsl:if>
    </td>
  </xsl:template>

  <xsl:template match="//Audio">
    <td>
      <xsl:for-each select="AudioTrack">
        <xsl:value-of select="AudioContent"/>
        <xsl:if test="position() != last()">, </xsl:if>
      </xsl:for-each>
    </td>
  </xsl:template>

  <xsl:template match="//Regions">
    <td>
      <xsl:for-each select="Region">
        <xsl:value-of select="text()"/>
        <xsl:if test="position() != last()">, </xsl:if>
      </xsl:for-each>
    </td>
  </xsl:template>

  <xsl:template match="//Subtitles">
    <td>
      <xsl:for-each select="Subtitle">
        <xsl:value-of select="text()"/>
        <xsl:if test="position() != last()">, </xsl:if>
      </xsl:for-each>
    </td>
  </xsl:template>

  <xsl:template name="OptionalElement">
    <xsl:param name="value"/>
    <td>
      <xsl:choose>
        <xsl:when test="$value != ''">
          <xsl:value-of select="$value"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:text>&#160;</xsl:text>
        </xsl:otherwise>
      </xsl:choose>
    </td>
  </xsl:template>

  <xsl:template match="/">
    <html>
      <head>
        <title>DVD Profiler Double Dips</title>
      </head>
      <body>
        <div>
          <h1>DVD Profiler Double Dips</h1>
        </div>
        <table border="1" cellpadding="3" cellspacing="0">
          <tr>
            <th>Title</th>
            <th>Edition</th>
            <th>Production Year</th>
            <th>Media Type</th>
            <!--<th>Purchase Date</th>
            <th>Collection Type</th>-->
            <th>Is Part of Box Set</th>
            <th>Original Title</th>
            <!--<th>Sort Title</th>
            <th>UPC / EAN</th>-->
            <th>Locality</th>
            <th>Regions</th>
            <th>Audio</th>
            <th>Subtitles</th>
          </tr>
          <xsl:for-each select="//Collection/DVD">
            <tr>
              <xsl:attribute name="style">
                <xsl:text>background-color:</xsl:text>
                <xsl:value-of select="Colour"/>
                <xsl:text>;</xsl:text>
              </xsl:attribute>
              <td>
                <xsl:value-of select="Title"/>
              </td>
              <xsl:call-template name="OptionalElement">
                <xsl:with-param name="value" select="DistTrait"/>
              </xsl:call-template>
              <td>
                <xsl:choose>
                  <xsl:when test="ProductionYear != '0'">
                    <xsl:value-of select="ProductionYear"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>&#160;</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <xsl:apply-templates select="MediaTypes"/>
              <!--<xsl:call-template name="OptionalElement">
                <xsl:with-param name="value" select="PurchaseInfo/PurchaseDate"/>
              </xsl:call-template>
              <xsl:call-template name="OptionalElement">
                <xsl:with-param name="value" select="CollectionType"/>
              </xsl:call-template>-->
              <td>
                <xsl:choose>
                  <xsl:when test="BoxSet/Parent != ''">
                    <xsl:text>&#x2713;</xsl:text>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>&#160;</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <xsl:call-template name="OptionalElement">
                <xsl:with-param name="value" select="OriginalTitle"/>
              </xsl:call-template>
              <!--<xsl:call-template name="OptionalElement">
                <xsl:with-param name="value" select="SortTitle"/>
              </xsl:call-template>
              <xsl:call-template name="OptionalElement">
                <xsl:with-param name="value" select="UPC"/>
              </xsl:call-template>-->
              <xsl:call-template name="OptionalElement">
                <xsl:with-param name="value" select="ID_LocalityDesc"/>
              </xsl:call-template>
              <xsl:apply-templates select="Regions"/>
              <xsl:apply-templates select="Audio"/>
              <xsl:apply-templates select="Subtitles"/>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>