<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MapApplication.Web.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.9.0/build/reset/reset-min.css" />

	<link rel="stylesheet" href="Scripts/css/example3.css" />
     <script type="text/javascript" src="Scripts/jquery-2.0.3.js" ></script>
    <title>中國科技大學 結構物安全與防災中心</title>
    <script  type="text/javascript">
        function getQueryStrings() {
            var assoc = {};
            var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
            var queryString = location.search.substring(1);
            var keyValues = queryString.split('&');

            for (var i in keyValues) {
                var key = keyValues[i].split('=');
                if (key.length > 1) {
                    assoc[decode(key[0])] = decode(key[1]);
                }
            }

            return assoc;
        }

        function changeImage() {
            var q = getQueryStrings();
            var id = q["id"];
           
          //  alert(document.getElementById('divpic').style.backgroundImage);
                if (id !=null) {
                 //   alert(document.getElementById("divpic").style.backgroundImage);           
                    document.getElementById("divpic").style.backgroundImage = "url('Images/" + id + ".jpg')";
                    document.getElementById("customerid").value = id;
                }
                else
                    document.getElementById("divpic").style.backgroundImage = "url('Images/final.jpg')";

              // alert(document.getElementById('divpic').style.backgroundImage);
        }
    </script>
</head>
<body onload="changeImage();"  >
    <form id="form1" runat="server">
    <center>
    <div  id="divpic" style="height:625px; width:1000px; background-image: url(Images/final.jpg); background-repeat:no-repeat;">
        <table cellpadding="0" cellspacing="0" width="100%" height="100%">
            <tr>
                <td colspan="2" height="115">&nbsp;</td>
            </tr>
            <tr height="225">
                <td width="70">&nbsp;</td>
                <td width="230">&nbsp;</td>
                <td width="45">&nbsp;</td>
                <td rowspan="2" valign="top">
                    <object id="FlashID" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="588" height="368"  style="margin-left:-60px;">
                        <param name="movie" value="Images/move.swf" />
                        <param name="quality" value="high" />
                        <param name="wmode" value="opaque" />
                        <param name="HasElevatedPermissions" value="true" />
                        <param name="swfversion" value="9.0.45.0" />
                    </object>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td valign="top">
                    <center><span style="color: #009; font-family: 微軟正黑體; font-weight: bold; font-size: 14pt;">使用者登入</span><br /><font style="font-size:4pt;">&nbsp;</font><br /></center>
                    <span style="color: #009; font-family: 微軟正黑體; font-weight: bold; font-size: 14pt;">&nbsp;&nbsp;帳號&nbsp;</span>
                    <asp:TextBox ID="Txt_MemberNo" runat="server"></asp:TextBox><br /><font style="font-size:8pt;">&nbsp;</font><br />
                    <span style="color: #009; font-family: 微軟正黑體; font-weight: bold; font-size: 14pt;">&nbsp;&nbsp;密碼&nbsp;</span>
                    <asp:TextBox ID="Txt_Password" TextMode="Password" runat="server"></asp:TextBox><br /><font style="font-size:8pt;">&nbsp;</font><br />
                    <center><asp:Button ID="Btn_Login" runat="server" Text="登入" 
                            onclick="Btn_Login_Click" />
                    <input type="hidden" runat="server"  id="customerid" />
                    <input type="button" name="button" id="Submit1" value="忘記密碼" onclick="javascript:location.href='ChangePasswordPage.aspx'"     /></center>
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    </center>
    </form>
</body>
</html>
