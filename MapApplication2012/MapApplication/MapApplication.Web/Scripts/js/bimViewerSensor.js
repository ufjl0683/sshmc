///#region  GlobalAttribute
var filePath = "http://192.192.161.4/mobile/Resources/0006.dwf";
var filePath_2 = "http://192.192.161.4/mobile/Resources/rjdj009sfsgr.dwf";


//var arrayName = ["sensor000035", "sensor000036", "sensor000039", "sensor000040"];
var arrayName = new Array();
var arraySensor = new Object();
///#endregion

//setViewer_Button
function setViewer() {
    var sensorCount = 0;
    var c = document.getElementById('ADViewer');
    var d = c.ECompositeViewer;
    c.Viewer.WaitForPageLoaded();
    d.ExecuteCommand('SHOWALL');
    var e = c.ECompositeViewer.Commands;
    e('TRANSPARENT').Enabled = true;
    e('INVERTSELECTION').Enabled = true;
    var f = d.Section.Content.Objects(0);
    var g = d.Section.Content.CreateUserCollection();
    for (var i = 1; i <= f.Count; i++) {
        var h = f(i).Properties;
        for (var j = 1; j <= h.Count; j++) {
            if (h(j).Name == 'URL') {
                //g.AddItem(f(i));
                arraySensor[sensorCount] = f(i);
                arrayName[sensorCount] = h(j).Value;
                //alert(sensorCount); //
                sensorCount++;
            }
        }
    }
    //d.Section.Content.Objects(1) = g;
}

//result_Button
function result(filePath) {

    $('#ADViewer').remove();
    $('.selectBtn').remove();
    $('#htmlhost').append('<embed id="ADViewer" style="width: 100%; height: 100%;" ' +
        'fullscreen=yes src="' + filePath + '" type="application/x-Autodesk-DWF" />');

    //將 embed 植入 #htmlhost 後呼叫 setViewer()
    setTimeout(function () {
        setViewer();
        //迴圈是用來產生 VSFinal範例內Button用，不使用可註解掉
        for (var i = 0; i < arrayName.length; i++) {
            var j = i + 1;
            $('#htmlhost').append('<button class="selectBtn" onclick="SelectObject(\'' + arrayName[i] + '\');">Sensor' + j + '\'' + arrayName[i] + '\'' + '</button>');
        }
    }, 2000);
}

function closeBim() {
    $('#ADViewer').remove();
    $('.selectBtn').remove();
}

//select_Button
function SelectObject(a) {
    console.log(a);
    var b = a.split(",");
    var c = document.getElementById('ADViewer');
    var d = c.ECompositeViewer;
    c.Viewer.WaitForPageLoaded();
    d.ExecuteCommand('SHOWALL');
    var e = c.ECompositeViewer.Commands;

    var addSensor = d.Section.Content.CreateUserCollection();
    for (var i = 0; i < 4; i++) {
        addSensor.AddItem(arraySensor[i]); //全域變數arraySensor[]放入addSensor
    }

    var f = addSensor;
    var g = d.Section.Content.CreateUserCollection();
    //每次呼叫SelectObject(a)方法，只會找尋arraySensor[]內的值
    for (var i = 1; i <= f.Count; i++) {
        var h = f(i).Properties;
        for (var j = 1; j <= h.Count; j++) {
            if (h(j).Name == 'URL' && $.inArray(h(j).Value, b) >= 0) {
                g.AddItem(f(i));
                //alert(i + "、" + j + "、" + h.Count);
            }
        }
    }
    d.Section.Content.Objects(1) = g;

    var section = c.ECompositeViewer.Section
    if (section.SectionType.Name == 'com.autodesk.dwf.eModel') {
        var camera = section.Camera;
        var curFiled = camera.Field;
        camera.Field.Set(80, 80);
        c.ECompositeViewer.Section.Camera = camera;
    }

    c.ExecuteCommand('FITTOWINDOW');
    e('MODELBAND').Toggled = false;
    e('SECTIONPROPERTIESBAND').Toggled = false;
    e('OBJECTPROPERTIESBAND').Toggled = false;
}