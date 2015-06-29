<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns="http://schemas.microsoft.com/wix/2006/wi"
    xmlns:wix="http://schemas.microsoft.com/wix/2006/wi"
    xmlns:util="http://schemas.microsoft.com/wix/UtilExtension">

    <xsl:template match="wix:Component[@Id='Thycotic.MemoryMq.SiteConnector.Service.exe']">
        <xsl:copy>
            <xsl:apply-templates select="@*" />
            <xsl:apply-templates select="*" />
            <ServiceInstall Arguments="runService -Installer.Version=$(var.InstallerVersion) -Pipeline.ConnectionString=[PIPELINE.CONNECTIONSTRING] -Pipeline.UseSsl=[PIPELINE.USESSL] -Pipeline.Thumbprint=[PIPELINE.THUMBPRINT]" Account="[SERVICE_USERNAME]" Password="[SERVICE_PASSWORD]" Id="ServiceInstallMemoryMqSiteConnectorService" Name="Thycotic.MemoryMq.SiteConnector.Service" Type="ownProcess" Start="auto" ErrorControl="normal" DisplayName="Thycotic MemoryMq Site Connector Service (x64)" Description="Allows site requests to be relayed to engines">
                <util:ServiceConfig FirstFailureActionType="restart" SecondFailureActionType="restart" ThirdFailureActionType="none" RestartServiceDelayInSeconds="60" ResetPeriodInDays="1" />
            </ServiceInstall>
            <ServiceControl Id="ServiceControlMemoryMqSiteConnectorService" Name="Thycotic.MemoryMq.SiteConnector.Service" Start="install" Stop="uninstall" Remove="uninstall"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="*">
        <xsl:copy>
            <xsl:apply-templates select="@*" />
            <xsl:apply-templates select="* | text()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="@* | text()">
        <xsl:copy />
    </xsl:template>

    <xsl:template match="/">
        <xsl:apply-templates />
    </xsl:template>

</xsl:stylesheet>