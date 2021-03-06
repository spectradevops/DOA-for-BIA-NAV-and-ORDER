﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace DOA
{
    class DOAHelper
    {

        public string getEmailBody(IOrganizationService service, string opptyid, string Approver)
        {
            string emailbody = null;
            try
            {
                string[] opptyattr = { "alletech_buildingname", "alletech_productsegment", "alletech_businesssegmentglb", "spectra_customersegmentcode", "ownerid", "parentaccountid", "alletech_oppurtunityid", "alletech_companynamebusiness", "alletech_redundancyrequired", "customerneed", "alletech_city", "alletech_area" };

                Entity Oppty = GetResultByAttribute(service, "opportunity", "opportunityid", opptyid, opptyattr);

                if (Oppty != null)
                {
                    string TempName = null; //used for all entity reference name values
                    string TempName2 = null;//used for Building status and billing frequency.
                    bool isMAC = false;

                    if (Oppty.Attributes.Contains("alletech_productsegment"))
                    {
                        string ProdSeg = Oppty.GetAttributeValue<EntityReference>("alletech_productsegment").Name;
                        if (ProdSeg != null && (ProdSeg.ToLower() == "metro access connect" || ProdSeg.ToLower() == "carrier access connect"))
                        {
                            isMAC = true;
                        }
                    }


                    #region Constructing HTML
                    emailbody = "<html><body><div>";
                    emailbody += "Hi " + Approver + ",<br/><br/>";
                    emailbody += "<p style = 'color: black; font-family: ' Arial',sans-serif; font-size: 10pt;'><b> Approval of price deviation for order : "+ Oppty.GetAttributeValue<string>("alletech_oppurtunityid") + " </b></p>";
                    emailbody += "<p style = 'color: black; font-family: ' Arial',sans-serif; font-size: 10pt;'><b> Order Details </b></p>";
                    emailbody += "<table class='MsoNormalTable' style='width:547pt;font-family: Arial,sans-serif; font-size: 9pt; border-collapse: collapse;' border='0' cellspacing='0' cellpadding='0'><tbody>";

                    #region Row 1
                    if (Oppty.Attributes.Contains("spectra_customersegmentcode"))
                        TempName = GetCustomersegment(Oppty.GetAttributeValue<OptionSetValue>("spectra_customersegmentcode").Value);
                    else
                        TempName = null;

                    emailbody += @"<tr style='height: 15pt;'>
                                <td width ='142' style='padding: 0in 5.4pt; border: 1pt solid windowtext; width: 106.35pt; height: 15pt; '>
                                <p><b> Business Segment </b></p></td>
                                <td width ='97' style='border-width: 1pt 1pt 1pt 0px; border-style: solid solid solid none;  padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>

                                <p align ='center' text-align: 'center;'>" + Oppty.GetAttributeValue<EntityReference>("alletech_businesssegmentglb").Name + @"</p></td>

                                <td width ='131' style='border-width: 1pt 1pt 1pt 0px; border-style: solid solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                                <p><b> Customer Segment </b></p></td>
                                <td width ='119' style='border-width: 1pt 1pt 1pt 0px; border-style: solid solid solid none;  padding: 0in 5.4pt; width: 89pt; height: 15pt; '>
                                <p align ='center' text-align: 'center;'>" + TempName + @"</p></td>
                                <td width ='93' style='border-width: 1pt 1pt 1pt 0px; border-style: solid solid solid none;  padding: 0in 5.4pt; width: 70pt; height: 15pt; '>
                                <p><b> A / C Manager </b></p></td>
                                <td width ='148' style='border-width: 1pt 1pt 1pt 0px; border-style: solid solid solid none;  padding: 0in 5.4pt; width: 111pt; height: 15pt; '>
                                <p align ='center' text-align: 'center;'>" + Oppty.GetAttributeValue<EntityReference>("ownerid").Name + @"</p></td>
                                </tr>";
                    #endregion

                    #region Row 2
                    if (Oppty.Attributes.Contains("parentaccountid"))
                        TempName = Oppty.GetAttributeValue<EntityReference>("parentaccountid").Name;
                    else
                        TempName = null;

                    if (isMAC)
                    {
                        emailbody += @"<tr style='height: 15pt;'>
                        <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt; height: 15pt;'>
                        <p><b>Customer Name</b></p></td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 72.65pt; height: 15pt;' colspan='2'>
                        <p align='center' text-align: center;'>" + TempName + @"</p></td>";

                        emailbody += @"<td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt;'>
                        <p><b>Opportunity ID</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 89pt; height: 15pt;' colspan='2'>
                            <p align='center' text-align: center;'>" + Oppty.GetAttributeValue<string>("alletech_oppurtunityid") + @"</p>
                        </td>";
                    }
                    else
                    {
                        emailbody += @"<tr style='height: 15pt;'>
                        <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt; height: 15pt;'>
                        <p><b>Customer Name</b></p></td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 72.65pt; height: 15pt;'>
                        <p align='center' text-align: center;'>" + TempName + @"</p></td>";

                        emailbody += @"<td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt;'>
                        <p><b>Opportunity ID</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 89pt; height: 15pt;'>
                            <p align='center' text-align: center;'>" + Oppty.GetAttributeValue<string>("alletech_oppurtunityid") + @"</p>
                        </td>";

                        if (Oppty.Attributes.Contains("alletech_city"))
                            TempName = Oppty.GetAttributeValue<EntityReference>("alletech_city").Name;
                        else
                            TempName = null;

                        emailbody += @"<td width='93' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 70pt; height: 15pt; '>
                        <p><b>City</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 111pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";
                    }

                    emailbody += "</tr>";
                    #endregion

                    if (isMAC)
                    {
                        #region End A

                        emailbody += @"<tr style='height: 15pt;'>
                        <td style='border-width: 1pt 1pt 1pt 1pt; border-style: solid solid solid solid;  padding: 0in 5.4pt; width: 89pt; height: 15pt; background-color: rgb(217, 217, 217);' colspan='6'><b>END A</b></td>
                        </tr>";

                        #region Row 3.1
                        emailbody += "<tr style='height: 15pt;'>";

                        if (Oppty.Attributes.Contains("alletech_city"))
                            TempName = Oppty.GetAttributeValue<EntityReference>("alletech_city").Name;
                        else
                            TempName = null;

                        emailbody += @"<td width='93' style='border-width: 1pt 1pt 1pt 1pt; border-style: solid solid solid solid;  padding: 0in 5.4pt; width: 70pt; height: 15pt; '>
                        <p><b>City</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 111pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";

                        if (Oppty.Attributes.Contains("alletech_area"))
                            TempName = Oppty.GetAttributeValue<EntityReference>("alletech_area").Name;
                        else
                            TempName = null;

                        emailbody += @"<td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt; height: 15pt; '>
                            <p><b>Area</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";


                        if (Oppty.Attributes.Contains("alletech_buildingname"))
                        {
                            TempName = Oppty.GetAttributeValue<EntityReference>("alletech_buildingname").Name;

                            Entity building = GetResultByAttribute(service, "alletech_building", "alletech_buildingid", Oppty.GetAttributeValue<EntityReference>("alletech_buildingname").Id.ToString(), "alletech_buildingstatus");

                            if (building != null && building.Attributes.Contains("alletech_buildingstatus"))
                            {
                                TempName2 = getBuildingStatusValue(building.GetAttributeValue<OptionSetValue>("alletech_buildingstatus").Value);
                            }
                        }
                        else
                        {
                            TempName = null;
                            TempName2 = null;
                        }

                        emailbody += @" <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                        <p><b>Building Name</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 89pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>
                        </tr>";
                        #endregion

                        #region Row 3.2
                        emailbody += @"<tr style='height: 15pt;'>";

                        emailbody += @"<td width='93' style='border-width: 1pt 1pt 1pt 1pt; border-style: solid solid solid solid;  padding: 0in 5.4pt; width: 70pt; height: 15pt; '>
                        <p><b>Building Status</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 111pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName2 + @"</p>
                        </td>";

                        if (Oppty.Attributes.Contains("alletech_redundancyrequired") && Oppty.GetAttributeValue<bool>("alletech_redundancyrequired"))
                            TempName = "Yes";
                        else
                            TempName = "No";

                        emailbody += @"<td  style='border-width: 0px 0px 1pt 1pt; border-style: none none solid solid;padding: 0in 5.4pt; width: 106.35pt; height: 15pt; '>
                        <p><b>Redundancy required</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid;padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";

                        if (TempName2 == "Non-RFS")
                        {
                            decimal amt = 0;
                            string[] feasiblityattr = { "alletech_feasibilityid" };
                            EntityCollection feasibleColl = GetResultsByAttributes(service, "alletech_feasibility", "alletech_opportunity", opptyid,"pcl_addresstype","1", feasiblityattr);

                            if (feasibleColl.Entities.Count > 0)
                            {
                                foreach (Entity feasible in feasibleColl.Entities)
                                {
                                    string[] Prjattr = { "alletech_pjcid", "alletech_totalcalculatedcost" };
                                    EntityCollection ProjcstColl = GetResultsByAttribute(service, "alletech_projectestimationcost", "alletech_pjcid", feasible.Id.ToString(), Prjattr);
                                    if (ProjcstColl.Entities.Count > 0)
                                    {
                                        foreach (Entity prj in ProjcstColl.Entities)
                                        {
                                            if (prj.Attributes.Contains("alletech_totalcalculatedcost"))
                                                amt += prj.GetAttributeValue<Money>("alletech_totalcalculatedcost").Value;
                                        }
                                    }
                                    if (TempName == "No")
                                        break;
                                }
                            }
                            TempName = amt.ToString();
                        }
                        else
                            TempName = null;

                        emailbody += @" <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                        <p><b>Connectivity Cost</b></p>
                        </td>
                        <td width='360' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none; border-color: rgb(0, 0, 0) black windowtext rgb(0, 0, 0); padding: 0in 5.4pt; width: 3.75in; height: 15pt;'>
                            <p align='center' text-align: center;' style='font-size: 8pt;'>" + TempName + @"</p>
                        </td>
                        </tr>";
                        #endregion

                        #endregion

                        #region End B
                        emailbody += @"<tr style='height: 15pt;'>
                        <td style='border-width: 1pt 1pt 1pt 1pt; border-style: solid solid solid solid;  padding: 0in 5.4pt; width: 89pt; height: 15pt; background-color: rgb(217, 217, 217);' colspan='6'><b>END B</b></td>
                        </tr>";

                        Entity END_B = GetResultByAttribute(service, "alletech_installationaddress", "alletech_opportunity", opptyid, "all");

                        #region Row 4.1
                        emailbody += "<tr style='height: 15pt;'>";

                        if (END_B.Attributes.Contains("alletech_city"))
                             TempName = END_B.GetAttributeValue<EntityReference>("alletech_city").Name;
                        else
                            TempName = null;

                        emailbody += @"<td width='93' style='border-width: 1pt 1pt 1pt 1pt; border-style: solid solid solid solid;  padding: 0in 5.4pt; width: 70pt; height: 15pt; '>
                        <p><b>City</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 111pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";

                        if (END_B.Attributes.Contains("alletech_area"))
                            TempName = END_B.GetAttributeValue<EntityReference>("alletech_area").Name;
                        else
                            TempName = null;

                        emailbody += @"<td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt; height: 15pt; '>
                            <p><b>Area</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";


                        if (END_B.Attributes.Contains("alletech_buildingnamelookup"))
                        {
                            TempName = END_B.GetAttributeValue<EntityReference>("alletech_buildingnamelookup").Name;

                            Entity building = GetResultByAttribute(service, "alletech_building", "alletech_buildingid", END_B.GetAttributeValue<EntityReference>("alletech_buildingnamelookup").Id.ToString(), "alletech_buildingstatus");

                            if (building != null && building.Attributes.Contains("alletech_buildingstatus"))
                            {
                                TempName2 = getBuildingStatusValue(building.GetAttributeValue<OptionSetValue>("alletech_buildingstatus").Value);
                            }
                        }
                        else
                        {
                            TempName = null;
                            TempName2 = null;
                        }

                        emailbody += @" <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                        <p><b>Building Name</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 89pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td></tr>";
                        #endregion

                        #region Row 4.2
                        emailbody += "<tr style='height: 15pt;'>";

                        emailbody += @"<td width='93' style='border-width: 1pt 1pt 1pt 1pt; border-style: solid solid solid solid;  padding: 0in 5.4pt; width: 70pt; height: 15pt; '>
                        <p><b>Building Status</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 111pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName2 + @"</p>
                        </td>";

                        if (Oppty.Attributes.Contains("alletech_redundancyrequired") && Oppty.GetAttributeValue<bool>("alletech_redundancyrequired"))
                            TempName = "Yes";
                        else
                            TempName = "No";

                        emailbody += @"<td  style='border-width: 0px 0px 1pt 1pt; border-style: none none solid solid;padding: 0in 5.4pt; width: 106.35pt; height: 15pt; '>
                        <p><b>Redundancy required</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid;padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";

                        if (TempName2 == "Non-RFS")
                        {
                            decimal amt = 0;
                            string[] feasiblityattr = { "alletech_feasibilityid" };
                            EntityCollection feasibleColl = GetResultsByAttributes(service, "alletech_feasibility", "alletech_opportunity", opptyid, "pcl_addresstype", "2", feasiblityattr);

                            if (feasibleColl.Entities.Count > 0)
                            {
                                foreach (Entity feasible in feasibleColl.Entities)
                                {
                                    string[] Prjattr = { "alletech_pjcid", "alletech_totalcalculatedcost" };
                                    EntityCollection ProjcstColl = GetResultsByAttribute(service, "alletech_projectestimationcost", "alletech_pjcid", feasible.Id.ToString(), Prjattr);
                                    if (ProjcstColl.Entities.Count > 0)
                                    {
                                        foreach (Entity prj in ProjcstColl.Entities)
                                        {
                                            if (prj.Attributes.Contains("alletech_totalcalculatedcost"))
                                                amt += prj.GetAttributeValue<Money>("alletech_totalcalculatedcost").Value;
                                        }
                                    }
                                    if (TempName == "No")
                                        break;
                                }
                            }
                            TempName = amt.ToString();
                        }
                        else
                            TempName = null;

                        emailbody += @" <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                        <p><b>Connectivity Cost</b></p>
                        </td>
                        <td width='360' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none; border-color: rgb(0, 0, 0) black windowtext rgb(0, 0, 0); padding: 0in 5.4pt; width: 3.75in; height: 15pt;'>
                            <p align='center' text-align: center;' style='font-size: 8pt;'>" + TempName + @"</p>
                        </td>
                        </tr>";
                        #endregion

                        #endregion 
                    }
                    else
                    {
                        #region Row 3
                        if (Oppty.Attributes.Contains("alletech_area"))
                            TempName = Oppty.GetAttributeValue<EntityReference>("alletech_area").Name;
                        else
                            TempName = null;

                        emailbody += @"<tr style='height: 15pt;'>
                        <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt; height: 15pt; '>
                            <p><b>Area</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";


                        if (Oppty.Attributes.Contains("alletech_buildingname"))
                        {
                            TempName = Oppty.GetAttributeValue<EntityReference>("alletech_buildingname").Name;

                            Entity building = GetResultByAttribute(service, "alletech_building", "alletech_buildingid", Oppty.GetAttributeValue<EntityReference>("alletech_buildingname").Id.ToString(), "alletech_buildingstatus");

                            if (building != null && building.Attributes.Contains("alletech_buildingstatus"))
                            {
                                TempName2 = getBuildingStatusValue(building.GetAttributeValue<OptionSetValue>("alletech_buildingstatus").Value);
                            }
                        }
                        else
                            TempName = null;

                        emailbody += @" <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                        <p><b>Building Name</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 89pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";

                        if (Oppty.Attributes.Contains("alletech_city"))
                            TempName = Oppty.GetAttributeValue<EntityReference>("alletech_city").Name;
                        else
                            TempName = null;

                        emailbody += @"<td width='93' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 70pt; height: 15pt; '>
                        <p><b>Building Status</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 111pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName2 + @"</p>
                        </td>
                        </tr>";
                        #endregion

                        #region Row 4
                        if (Oppty.Attributes.Contains("alletech_redundancyrequired") && Oppty.GetAttributeValue<bool>("alletech_redundancyrequired"))
                            TempName = "Yes";
                        else
                            TempName = "No";

                        emailbody += @"<tr style='height: 15pt;'>
                        <td  style='border-width: 0px 0px 1pt 1pt; border-style: none none solid solid;padding: 0in 5.4pt; width: 106.35pt; height: 15pt; '>
                            <p><b>Redundancy required</b></p>
                        </td>
                        <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid;padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>
                            <p align='center' text-align: center;'>" + TempName + @"</p>
                        </td>";

                        if (TempName2 == "Non-RFS")
                        {
                            decimal amt = 0;
                            string[] feasiblityattr = { "alletech_feasibilityid" };
                            EntityCollection feasibleColl = GetResultsByAttribute(service, "alletech_feasibility", "alletech_opportunity", opptyid, feasiblityattr);

                            if (feasibleColl.Entities.Count > 0)
                            {
                                foreach (Entity feasible in feasibleColl.Entities)
                                {
                                    string[] Prjattr = { "alletech_pjcid", "alletech_totalcalculatedcost" };
                                    EntityCollection ProjcstColl = GetResultsByAttribute(service, "alletech_projectestimationcost", "alletech_pjcid", feasible.Id.ToString(), Prjattr);
                                    if (ProjcstColl.Entities.Count > 0)
                                    {
                                        foreach (Entity prj in ProjcstColl.Entities)
                                        {
                                            if (prj.Attributes.Contains("alletech_totalcalculatedcost"))
                                                amt += prj.GetAttributeValue<Money>("alletech_totalcalculatedcost").Value;
                                        }
                                    }
                                    if (TempName == "No")
                                        break;
                                }
                            }
                            TempName = amt.ToString();
                        }
                        else
                            TempName = null;

                        emailbody += @" <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                        <p><b>Connectivity Cost</b></p>
                    </td>
                    <td width='360' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none; border-color: rgb(0, 0, 0) black windowtext rgb(0, 0, 0); padding: 0in 5.4pt; width: 3.75in; height: 15pt; ' colspan='3'>
                        <p align='center' text-align: center;' style='font-size: 8pt;'>" + TempName + @"</p>
                    </td>
                </tr>";
                        #endregion
                    }

                    #region Checking OpptyProds
                    string[] Opptyvalues = new string[4];
                    TempName = null; TempName2 = null;
                    string opptysubtable = "";

                    string[] opptyprodattr = { "productid", "priceperunit", "manualdiscountamount", "extendedamount" };
                    EntityCollection opptyProds = GetResultsByAttribute(service, "opportunityproduct", "opportunityid", opptyid, opptyprodattr);

                    if (opptyProds.Entities.Count > 0)
                    {
                        foreach (Entity opptyprod in opptyProds.Entities)
                        {

                            string prodname = opptyprod.GetAttributeValue<EntityReference>("productid").Name;
                            try
                            {
                                string[] prodattr = { "alletech_billingcycle", "alletech_plantype", "alletech_chargetype" };
                                Entity prod = GetResultByAttribute(service, "product", "productid", opptyprod.GetAttributeValue<EntityReference>("productid").Id.ToString(), prodattr);

                                int plan = prod.GetAttributeValue<OptionSetValue>("alletech_plantype").Value;
                                int charge = prod.GetAttributeValue<OptionSetValue>("alletech_chargetype").Value;

                                decimal price = 0, extnamt = 0, percentAge = 0;

                                //RC||OTC||Addon
                                if (charge == 569480001 || charge == 569480002 || plan == 569480002) //addon plan type
                                {
                                    price = opptyprod.GetAttributeValue<Money>("priceperunit").Value;
                                    Opptyvalues[0] = price.ToString();

                                    if (opptyprod.Contains("manualdiscountamount"))
                                        Opptyvalues[1] = opptyprod.GetAttributeValue<Money>("manualdiscountamount").Value.ToString();
                                    else
                                        Opptyvalues[1] = "0";

                                    extnamt = opptyprod.GetAttributeValue<Money>("extendedamount").Value;
                                    Opptyvalues[2] = extnamt.ToString();

                                    if (extnamt < price)
                                    {
                                        percentAge = (price - extnamt) / price * 100;
                                        percentAge = decimal.Round(percentAge, 2, MidpointRounding.AwayFromZero);
                                    }

                                    Opptyvalues[3] = percentAge.ToString() + "%";

                                    opptysubtable += "<tr style='height: 15pt;'>";
                                    opptysubtable += "<td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt;'> ";
                                    opptysubtable += "<p align='center' text-align: center;'><b>" + prodname + "</b></p>";
                                    opptysubtable += "</td>";
                                    opptysubtable += "<td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 72.65pt;'> ";
                                    opptysubtable += "<p align='center' text-align: center;'>" + Opptyvalues[0] + @"</p>";
                                    opptysubtable += "</td>";
                                    opptysubtable += "<td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt;'> ";
                                    opptysubtable += "<p align='center' text-align: center;'>" + Opptyvalues[1] + @"</p>";
                                    opptysubtable += "</td>";
                                    opptysubtable += "<td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 89pt;'> ";
                                    opptysubtable += "<p align='center' text-align: center;'>" + Opptyvalues[2] + @"</p>";
                                    opptysubtable += "</td>";
                                    opptysubtable += "<td width='241' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 181pt;' colspan='2'> ";
                                    opptysubtable += "<p align='center' text-align: center;'>" + Opptyvalues[3] + @"</p>";
                                    opptysubtable += "</td>";
                                    opptysubtable += "</tr>";
                                }
                                else
                                {
                                    TempName = prodname;

                                    if (prod.Attributes.Contains("alletech_billingcycle"))
                                        TempName2 = prod.GetAttributeValue<EntityReference>("alletech_billingcycle").Name;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    #endregion

                    #region Row 5
                    emailbody += @"<tr style='height: 15pt;'>
                    <td width='239' style='border-width: 0px 1pt 1pt; border-style: none solid solid; border-color: rgb(0, 0, 0) black windowtext windowtext; padding: 0in 5.4pt; width: 179pt; height: 15pt;' >
                        <p align='center' text-align: center;'><b>Product</b></p>
                    </td>
                    <td  style='border-width: 0px 0px 1pt; border-style: none none solid; border-color: rgb(0, 0, 0) rgb(0, 0, 0) windowtext; padding: 0in 5.4pt; width: 98pt; height: 15pt;' colspan='2'>
                        <p align='center' text-align: center;'>" + TempName + @"</p>
                    </td>
                    <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 89pt; height: 15pt; '>
                        <p><b>Billing Frequency</b></p>
                    </td>
                    <td width='241' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none; border-color: rgb(0, 0, 0) black windowtext rgb(0, 0, 0); padding: 0in 5.4pt; width: 181pt; height: 15pt;' colspan='2'>
                        <p align='center' text-align: center;'>" + TempName2 + @"</p>
                    </td>
                    </tr>";
                    #endregion

                    #region Row 6
                    emailbody += @"<tr style='height: 15pt;'>
                    <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt; height: 15pt; '>
                        <p align='center' text-align: center;'><b>Product Component</b></p>
                    </td>
                    <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 72.65pt; height: 15pt; '>
                        <p align='center' text-align: center;'><b>Floor Price</b></p>
                    </td>
                    <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 98pt; height: 15pt; '>
                        <p align='center' text-align: center;'><b>Discount</b></p>
                    </td>
                    <td  style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 89pt; height: 15pt; '>
                        <p align='center' text-align: center;'><b>Discounted Price</b></p>
                    </td>
                    <td width='241' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 181pt; height: 15pt; ' colspan='2'>
                        <p align='center' text-align: center;'><b>Price Deviation %</b></p>
                    </td>
                    </tr>";
                    #endregion

                    emailbody += opptysubtable; //Row 7 and 8

                    #region Row 9

                    emailbody += @"<tr style='height: 33pt;'>
                    <td  style='border-width: 0px 1pt 1pt; border-style: none solid solid; padding: 0in 5.4pt; width: 106.35pt; height: 33pt; '>
                        <p><b>Remarks</b></p>
                    </td>
                    <td width='588' style='border-width: 0px 1pt 1pt 0px; border-style: none solid solid none;  padding: 0in 5.4pt; width: 440.65pt; height: 33pt; ' colspan='5'>
                        <p align='center' text-align: center;'>" + Oppty.GetAttributeValue<string>("customerneed") + @"</p>
                    </td>
                    </tr>";
                    #endregion

                    emailbody += "</tbody></table>";
                    emailbody += "<br /> Note: To approve from email, please reply to this email with just 'Approve'/'Approved'.";
                    emailbody += "<br />       To reject from email, please reply to this email with just 'Reject'/'Rejected'.";
                    emailbody += "<br /><br /> Regards,<br /> Spectra Team <br />";
                    emailbody += "</div></body></html>";

                    #endregion
                }
            }
            catch (Exception ex)
            {
                emailbody = "Exception In creating  body : " + ex.Message;
            }
            return emailbody;
        }

        public string GetCustomersegment(int value)
        {
            string text = null;

            switch (value)
            {
                case 111260000: text = "SMB"; break;
                case 111260001: text = "Media"; break;
                case 111260002: text = "LA"; break;
                case 111260003: text = "SP"; break;
            }
            return text;
        }

        public string getBuildingStatusValue(int value)
        {
            string text = null;

            switch (value)
            {
                case 1: text = "Non-RFS"; break;
                case 2: text = "B-RFS"; break;
                case 3: text = "P-RFS"; break;
                case 4: text = "C-RFS"; break;
                case 5: text = "A-RFS Type1"; break;
                case 6: text = "A-RFS Type2"; break;
                case 7: text = "Pb-RFS"; break;
                case 8: text = "L2P-RFS"; break;
                case 9: text = "L2B-RFS"; break;
            }
            return text;
        }

        public Entity GetResultByAttribute(IOrganizationService _service, string entityName, string attrName, string attrValue, string column)
        {
            try
            {
                Entity result = null;
                QueryExpression query = new QueryExpression(entityName);

                query.NoLock = true;

                if (column == "all")
                    query.ColumnSet.AllColumns = true;
                else
                    query.ColumnSet.AddColumns(column);

                query.Criteria.AddCondition(attrName, ConditionOperator.Equal, attrValue);

                if (entityName != "systemuser")
                    query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

                EntityCollection resultcollection = _service.RetrieveMultiple(query);

                if (resultcollection.Entities.Count > 0)
                    return resultcollection.Entities[0];
                else
                    return result;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        public Entity GetResultByAttribute(IOrganizationService _service, string entityName, string attrName, string attrValue, string[] column)
        {
            try
            {
                Entity result = null;
                QueryExpression query = new QueryExpression(entityName);

                query.NoLock = true;
                query.ColumnSet.AddColumns(column);
                query.Criteria.AddCondition(attrName, ConditionOperator.Equal, attrValue);

                //query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

                EntityCollection resultcollection = _service.RetrieveMultiple(query);

                if (resultcollection.Entities.Count > 0)
                    return resultcollection.Entities[0];
                else
                    return result;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

        public EntityCollection GetResultsByAttributes(IOrganizationService _service, string entityName, string attrName1, string attrValue1, string attrName2, string attrValue2, string[] columns)
        {
            EntityCollection recordsCollection = new EntityCollection();

            try
            {
                QueryExpression query = new QueryExpression(entityName);
                query.ColumnSet.AddColumns(columns);
                query.Criteria.AddCondition(attrName1, ConditionOperator.Equal, attrValue1);

                if (attrName2 != null)
                    query.Criteria.AddCondition(attrName2, ConditionOperator.Equal, attrValue2);

                int pageNumber = 1;
                RetrieveMultipleRequest multiRequest;
                RetrieveMultipleResponse multiResponse = new RetrieveMultipleResponse();

                do
                {
                    query.PageInfo.Count = 5000;
                    query.PageInfo.PagingCookie = (pageNumber == 1) ? null : multiResponse.EntityCollection.PagingCookie;
                    query.PageInfo.PageNumber = pageNumber++;

                    multiRequest = new RetrieveMultipleRequest();
                    multiRequest.Query = query;
                    multiResponse = (RetrieveMultipleResponse)_service.Execute(multiRequest);

                    recordsCollection.Entities.AddRange(multiResponse.EntityCollection.Entities);
                }
                while (multiResponse.EntityCollection.MoreRecords);

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }

            return recordsCollection;
        }

        public EntityCollection GetResultsByAttribute(IOrganizationService _service, string entityName, string attrName, string attrValue, string[] column)
        {
            try
            {
                QueryExpression query = new QueryExpression(entityName);
                query.NoLock = true;
                query.ColumnSet.AddColumns(column);
                query.Criteria.AddCondition(attrName, ConditionOperator.Equal, attrValue);
                //query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

                EntityCollection resultcollection = _service.RetrieveMultiple(query);

                return resultcollection;

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }

    }
}
