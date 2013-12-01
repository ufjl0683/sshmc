<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="MapApplication.Web.News" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      
	<link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.9.0/build/reset/reset-min.css" />

	<link rel="stylesheet" href="Scripts/css/example3.css" />
     <script type="text/javascript" src="Scripts/jquery-2.0.3.js" ></script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    	<div class="wrapper"  id="wrapper" >

		<div class="header">

			<h1>The SSHMC.Cute Times</h1>

		</div><!--End Header -->

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

	<!--	<div class="main">

			<h2>This is a subheading Title</h2>
            
			<p>Cum porta risus odio tincidunt? Ultrices, nascetur lundium tincidunt ridiculus? Enim nisi ac nec nec vel in ac pid ultricies nunc, cras porta ultrices. Hac lacus nisi non! Ut turpis, tempor eros? Tortor et, platea turpis scelerisque duis porttitor augue ridiculus nec et mid, sociis? Est ac a? Arcu elementum eros in ultrices dignissim pulvinar tincidunt elementum, hac platea hac. Purus et egestas vel, elementum facilisis, turpis sit augue ultricies, porta augue, enim massa cursus ac augue! Porta sed cras. Ut a augue ac in, dis pulvinar? Cras amet aenean magna adipiscing turpis mattis purus placerat, placerat nunc placerat urna.</p>
			
		</div> 
        <div class="main2">
            <h2>cute</h2>
            <h6>2013/4/1</h6>
            <p>
                2013/4/1 12:00:00", "蘋果迷照過來，iPhone5S和5C第一批水貨在台灣落地，東森新聞獨家取得實測，首次開賣的5S金色版，從機身到包裝盒都燙金，難怪價格昂貴，水貨喊價飆到7萬元，而5C的塑膠外殼，表面質感相當光滑，但要價也不便宜，水貨一隻2萬6起跳。香檳金的邊框與背部機身，連包裝盒也是貴氣十足的燙金邊，想搶先擁有一隻64G版本價格，已經喊到7萬天價。就算價格再貴，台灣的蘋果迷們也迫不急待一睹5S真面目。不只5S，首波iPhone5c也在東森新聞獨家曝光，長的跟iphone5差不多，只是穿上多彩的塑膠外。但5C水貨也不便宜，16G 2萬6，32G則要3萬元，看來不管5S還是5C，果迷們想搶先擁有新機，口袋得夠深才行。
            </p>
        </div>-->
		<!--<div class="main2">

			<h2>This is a subheading Title that Spans all the Way Across The Columns</h2>
            <h6>2013/4/1 12:00</h6>
            <img src="http://ads.yimg.com/ja/a/tw/mall/ecm/p091537488468_1.jpg" style="float:left;width:200px;height:200px" />
			<p >Cum porta risus odio tincidunt? Ultrices, nascetur lundium tincidunt ridiculus? Enim nisi ac nec nec vel in ac pid ultricies nunc, cras porta ultrices. Hac lacus nisi non! Ut turpis, tempor eros? Tortor et, platea turpis scelerisque duis porttitor augue ridiculus nec et mid, sociis? Est ac a? Arcu elementum eros in ultrices dignissim pulvinar tincidunt elementum, hac platea hac. Purus et egestas vel, elementum facilisis, turpis sit augue ultricies, porta augue, enim massa cursus ac augue! Porta sed cras. Ut a augue ac in, dis pulvinar? Cras amet aenean magna adipiscing turpis mattis purus placerat, placerat nunc placerat urna. Mus porttitor in a nec, augue! Elit nisi purus? Porta platea integer.  Elit nisi purus? Porta platea integer.</p>
			
		</div> 
<div class="main2">

			<h2>This is a subheading Title that Spans all the Way Across The Columns</h2>
			<p>Cum porta risus odio tincidunt? Ultrices, nascetur lundium tincidunt ridiculus? Enim nisi ac nec nec vel in ac pid ultricies nunc, cras porta ultrices. Hac lacus nisi non! Ut turpis, tempor eros? Tortor et, platea turpis scelerisque duis porttitor augue ridiculus nec et mid, sociis? Est ac a? Arcu elementum eros in ultrices dignissim pulvinar tincidunt elementum, hac platea hac. Purus et egestas vel, elementum facilisis, turpis sit augue ultricies, porta augue, enim massa cursus ac augue! Porta sed cras. Ut a augue ac in, dis pulvinar? Cras amet aenean magna adipiscing turpis mattis purus placerat, placerat nunc placerat urna. Mus porttitor in a nec, augue! Elit nisi purus? Porta platea integer.  Elit nisi purus? Porta platea integer.</p>
			
		</div> 
        <div class="main2">

			<h2>This is a subheading Title that Spans all the Way Across The Columns</h2>
			<p>Cum porta risus odio tincidunt? Ultrices, nascetur lundium tincidunt ridiculus? Enim nisi ac nec nec vel in ac pid ultricies nunc, cras porta ultrices. Hac lacus nisi non! Ut turpis, tempor eros? Tortor et, platea turpis scelerisque duis porttitor augue ridiculus nec et mid, sociis? Est ac a? Arcu elementum eros in ultrices dignissim pulvinar tincidunt elementum, hac platea hac. Purus et egestas vel, elementum facilisis, turpis sit augue ultricies, porta augue, enim massa cursus ac augue! Porta sed cras. Ut a augue ac in, dis pulvinar? Cras amet aenean magna adipiscing turpis mattis purus placerat, placerat nunc placerat urna. Mus porttitor in a nec, augue! Elit nisi purus? Porta platea integer.  Elit nisi purus? Porta platea integer.</p>
			
		</div> 
		<div class="main3">

			<h2>This is a subheading Title</h2>
			<p>Cum porta risus odio tincidunt? Ultrices, nascetur lundium tincidunt ridiculus? Enim nisi ac nec nec vel in ac pid ultricies nunc, cras porta ultrices. Hac lacus nisi non! Ut turpis, tempor eros? Tortor et, platea turpis scelerisque duis porttitor augue ridiculus nec et mid, sociis? Est ac a? Arcu elementum eros in ultrices dignissim pulvinar tincidunt elementum, hac platea hac. Purus et egestas vel, elementum facilisis, turpis sit augue ultricies, porta augue, enim massa cursus ac augue! Porta sed cras. Ut a augue ac in, dis pulvinar? Cras amet aenean magna adipiscing turpis mattis purus placerat, placerat nunc placerat urna. Mus porttitor in a nec, augue! Elit nisi purus? Porta platea integer.  Elit nisi purus? Porta platea integer.</p>
			
		</div> 
		
		<div class="side">

			<h2>This is the side</h2>
			<p>Turpis purus aliquet. Habitasse placerat odio ac sociis mid, auctor cursus parturient eros! Lundium, vut, urna ridiculus? Penatibus ac et mus elementum. Sed magna augue, velit odio magna pellentesque a, lacus, aliquam elementum, in massa! Dapibus turpis, placerat purus, lectus in sagittis porta massa aenean! Turpis enim non adipiscing lundium parturient, a natoque egestas. Mid dictumst porta. Risus lorem aliquam velit mid, placerat! Et lectus sociis scelerisque ac turpis. Magna porta? Augue, augue in. Porta dolor, et amet, ac. Cum scelerisque tincidunt dignissim in, pellentesque habitasse! Elit? Mus porttitor in a nec, augue! Elit nisi purus? Porta platea integer. Mus porttitor in a nec, augue! Elit nisi purus? Porta platea integer.</p>
			
		</div> -->

		<div class="footer" id="footer1">

			<p>&copy; sshmc 2011, All Rights Reserved</p>
			
		</div><!-- / footer -->

	</div><!-- / Wrapper -->
    </div>
    </form>
</body>
</html>
