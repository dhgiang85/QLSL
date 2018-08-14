var reportController = function () {
    var cachedObj = {
        //MONTHS : ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
        MONTHS: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']
    }
    this.initialize = function () {
        
        $.when()
            .done(function () {   
                loadMyBarChart();
                loadPolarAreaChart();
                loadDonutChart();
            });
 
        registerEvents()

      
    }
    function loadDonutChart() {
        var canvas = document.getElementById("Doughnut");
        var ctx = canvas.getContext("2d");
        var cx = canvas.width / 2;
        var cy = canvas.height / 2;
 
        $.ajax({
            type: "GET",
            url: "/Report/RateReport",
            data: {},
            dataType: "json",
            beforeSend: function () {

            },
            success: function (response) {
                var config = {
                    type: 'doughnut',
                    data: {
                        datasets: [{
                            data: [response.NonProcessErr, response.ProcessedErrTotal],
                            backgroundColor: [
                                "#F7464A",
                                "#46BFBD",
                    
                            ],
                            label: 'Expenditures'
                        }],
                        labels: [
                            "Tồn tại",
                            "Đã xử lý",

                        ]
                    },
                    options: {
                        responsive: true,
                        legend: {
                            position: 'bottom',
                        },
                        title: {
                            display: false,
                            text: 'Chart.js Doughnut Chart'
                        },
                        animation: {
                            animateScale: true,
                            animateRotate: true
                        }

                    },  
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                var dataset = data.datasets[tooltipItem.datasetIndex];
                                var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                                    return previousValue + currentValue;
                                });
                                var currentValue = dataset.data[tooltipItem.index];
                                var percentage = Math.floor(((currentValue / total) * 100) + 0.5);
                                return percentage + "%";
                            }
                        }
                    }
                };
                
                var DonutChart = new Chart(ctx, config);
 

            },
            error: function (status) {
                console.log(status);
            }
        });

    };

    function loadPolarAreaChart() {
        var ctx = document.getElementById("polarArea");
        $.ajax({
            type: "GET",
            url: "/Report/ErrorReport",
            data: {},
            dataType: "json",
            beforeSend: function () {

            },
            success: function (response) {
                console.log(response)
                var data = {
                    datasets: [{
                        data: [response.CCTVError, response.TFError, response.VMSError, response.WIMError],
                        backgroundColor: [
                            "rgba(9, 0, 255)",
                            "rgba(0, 255, 30)",
                            "rgba(3, 88, 106)",
                            "rgba(240, 255, 0)",
                        
                        ],
                        label: 'My dataset'
                    }],
                    labels: [
                        "Camera",
                        "Đèn tín hiệu",
                        "VMS",
                        "Trạm cân",
                     
                    ]
                };
                var polarArea = new Chart(ctx, {
                    data: data,
                    type: 'polarArea',
                    options: {
                        legend: {

                            position: 'right',
                        },
                        scale: {
                            ticks: {
                                beginAtZero: true
                            }
                        }
                    }
                });
               
            },
            error: function (status) {
                console.log(status);
            }
        });

    };
    function loadMyBarChart() {
        var barChartData = {
            labels: cachedObj.MONTHS,
            datasets: []
        };
        var ctx = document.getElementById('mybarChart').getContext('2d');
        var mybarChart = new Chart(ctx, {
            type: 'bar',
            data: barChartData,
            options: {
                responsive: true,
                legend: {
                    position: 'top',
                },
                title: {
                    display: false,
                    text: 'Lỗi hàng tháng'
                },
                maintainAspectRatio: false,
            }
        });

        $.ajax({
            type: "GET",
            url: "/Report/MonthlyReport",
            data: {
                Year: 2018,
               
            },
            dataType: "json",
            beforeSend: function () {
                
            },
            success: function (response) {
                console.log(response)
                
                var newDataset = {
                    label: response.MRCCTV.Label,
                    backgroundColor: "rgba(9, 0, 255, 0.8)",
                    borderColor: "rgba(9, 0, 255, 0.2)",
                    borderWidth: 1,
                    data: response.MRCCTV.TotalError
                };

                barChartData.datasets.push(newDataset);
                newDataset = {
                    label: response.MRTF.Label,
                    backgroundColor: "rgba(0, 255, 30, 0.8)",
                    borderColor: "rgba(0, 255, 30, 0.2)",
                    borderWidth: 1,
                    data: response.MRTF.TotalError
                };

                barChartData.datasets.push(newDataset);

               newDataset = {
                    label: response.MRVMS.Label,
                    backgroundColor: "rgba(3, 88, 106, 0.8)",
                    borderColor: "rgba(3, 88, 106, 0.2",
                    borderWidth: 1,
                    data: response.MRVMS.TotalError
                };
                barChartData.datasets.push(newDataset);

                newDataset = {
                    label: response.MRWIM.Label,
                    backgroundColor: "rgba(240, 255, 0, 0.8)",
                    borderColor: "rgba(240, 255, 0, 0.2)",
                    borderWidth: 1,
                    data: response.MRWIM.TotalError
                };
                barChartData.datasets.push(newDataset);
                mybarChart.update();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function registerEvents() {

        
    }
}