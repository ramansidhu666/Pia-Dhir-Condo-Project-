<%@ Page Title="" Language="C#" MasterPageFile="~/PropertyNew.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="MyListings.aspx.cs" Inherits="Property.MyListings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
 
<%@ Register TagName="ContactInfo" TagPrefix="uc" Src="~/Controls/ContactInfo.ascx" %>
<%@ Register TagName="SearchBar" TagPrefix="uc" Src="~/Controls/SearchBar.ascx" %>
<%@ Register Src="~/Controls/MyListingsControl.ascx" TagPrefix="uc" TagName="MyListingsControl" %>
 

<%@ Register Src="~/Controls/SearchControl.ascx" TagPrefix="uc" TagName="SearchControl" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <ajaxtoolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxtoolkit:ToolkitScriptManager>
    
    <div>
        <div class="left_section_bg">
            <div class="left_section_in_5">
                <div class="box5">
                <h2>My Exclusive Listings</h2>
                    <div class="In_bg">                       
                        <uc:MyListingsControl runat="server" id="MyListingsControl" />
                    </div>
                    <div class="In_bg">
                        
                   
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
