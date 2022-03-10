<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <!-- Mobile Specific Metas
        ================================================== -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="shortcut icon" href="/images/favicon.ico" type="image/x-icon">

    <!-- Google Web Fonts
        ================================================== -->
    <link href="http://fonts.googleapis.com/css?family=Roboto+Slab:700,500,400%7cCourgette%7cRoboto:400,500,700%7cCourgette%7cPT+Serif:400,700italic" rel="stylesheet" type="text/css">

    <!-- Theme CSS
        ================================================== -->
    <link rel="stylesheet" href="/css/normalize.css">
    <link rel="stylesheet" href="/css/styles.css">
    <link rel="stylesheet" href="/css/layout.css">

    <!-- Vendor CSS
        ================================================== -->
    <link rel="stylesheet" href="/css/vendor.css">
    <link rel="stylesheet" href="/css/fontello.css">


    <!-- additional by adham
        ================================================== -->
    <link rel="stylesheet" href="/css/additional.css">

    <link rel="stylesheet" href="//cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <script src="//code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="//cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>

    <script>
        $(document).ready(function () {
            function getDimKey(d) {
                return d.toLowerCase().replace(" ", "_");
            }
            $.ajax({
                dataType: "json",
                method: "POST",
                url: "https://unstats.un.org/SDGAPI/v1/sdg/Series/PivotData",
                data: { seriescode: "SI_POV_EMP1", areaCode: 275 },
                success: function (dataPivot) {
                    var data = [];
                    var columns = [];
                    for (var d in dataPivot.dimensions) {
                        var dim = dataPivot.dimensions[d];
                        columns.push({ title: dim.id });
                    }
                    var years = JSON.parse(dataPivot.data[0].years);
                    console.log(years);
                    for (var y in years) {
                        if (years[y].year.replace(/[\[\]']+/g,'') > 2003) {
                            columns.push({ title: years[y].year.replace(/[\[\]']+/g,'') });
                        }
                    }
                    for (var i in dataPivot.data) {
                        var dPivot = dataPivot.data[i];
                        var years = JSON.parse(dPivot.years);
                        var dd = [];
                        for (var d in dataPivot.dimensions) {
                            var dim = dataPivot.dimensions[d];
                            dd.push(dPivot[getDimKey(dim.id)]);
                        }
                        for (var y in years) {
                            if (years[y].year.replace(/[\[\]']+/g,'') > 2003) {
                                dd.push(years[y].value);
                            }
                        }
                        data.push(dd);
                    }
                    console.log(columns);
                    console.log(data);
                    $('#sdgTable').DataTable({
                        data: data,
                        columns: columns,
                        responsive: true,
                        paging: false,
                        searching: false
                    });
                }
            });
        });
    </script>
</head>
<body>
    <header id="header" class="header type-3">
        <div class="header-middle">
            <div class="row">
                <div class="large-12 columns">
                    <div class="header-middle-entry">
                        <div class="logo">
                            <span class="tmm_logo">
                                <a title="logo" href="http://www.pcbs.gov.ps/default.aspx">
                                    <img src="images/logowhite129.png" alt="logo" title="pcbs">
                                </a>
                            </span>
                            <span class="sop">State of Palestine</span><br>
                            <span class="sop2">Palestinian Central Bureau of Statistics</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <main id="content" class="row">
        <div class="large-12 columns">
            <div class="www" style="font-size: 16pt; margin-bottom: 20px;">
                <b>Sustainable Development Goals</b>
            </div>
        </div>
        <div class="large-12 columns">
            <asp:Literal runat="server" ID="htmlStr" EnableViewState="false" />
        </div>

        <table id="sdgTable" class="display compact nowrap" style="width:100%"></table>
    </main>
</body>
</html>
