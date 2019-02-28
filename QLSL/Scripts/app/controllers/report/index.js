var reportController = function () {
    var cachedObj = {
        //MONTHS : ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
        MONTHS: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12']
    }
    var ctx = document.getElementById('mybarChart').getContext('2d');

    var barChartData = {
        labels: cachedObj.MONTHS,
        datasets: []
    };

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

    this.initialize = function () {      
        $.when()
            .done(function () {
                init();
                loadMyBarChart();
                loadDonutChart();
                loadeCharts();
                
            });

        registerEvents();


    }

    function init() {       
        var year = (new Date()).getFullYear();
        document.getElementById('YearReport').value = year;
        var year_select = document.getElementById('YearReport');
        year_select.addEventListener('change', function () { loadMyBarChart(this.value); }, false);
    };
    function loadMyBarChart(value) {
       

        $.ajax({
            type: "GET",
            url: "/Report/MonthlyReport",
            data: {Year: value},
            dataType: "json",
            beforeSend: function () {

            },
            success: function (response) {
                //var newDataset = {
                //    label: response.MRCCTV.Label,
                //    backgroundColor: "rgba(9, 0, 255, 0.8)",
                //    borderColor: "rgba(9, 0, 255, 0.2)",
                //    borderWidth: 1,
                //    data: response.MRCCTV.TotalError
                //};

                //barChartData.datasets.push(newDataset);
                //newDataset = {
                //    label: response.MRTF.Label,
                //    backgroundColor: "rgba(3,148,71, 0.9)",
                //    borderColor: "rgba(3,148,71, 0.1)",
                //    borderWidth: 1,
                //    data: response.MRTF.TotalError
                //};

                //barChartData.datasets.push(newDataset);

                //newDataset = {
                //    label: response.MRVMS.Label,
                //    backgroundColor: "rgba(103,9,9, 0.9)",
                //    borderColor: "rgba(103,9,9, 0.1)",
                //    borderWidth: 1,
                //    data: response.MRVMS.TotalError
                //};
                //barChartData.datasets.push(newDataset);

                //newDataset = {
                //    label: response.MRWIM.Label,
                //    backgroundColor: "rgba(255, 101, 47, 0.9)",
                //    borderColor: "rgba(255, 101, 47, 0.1)",
                //    borderWidth: 1,
                //    data: response.MRWIM.TotalError
                //};
                //barChartData.datasets.push(newDataset);
                //mybarChart.update();
                var newDatasets = [{
                    label: response.MRCCTV.Label,
                    backgroundColor: "rgba(9, 0, 255, 0.8)",
                    borderColor: "rgba(9, 0, 255, 0.2)",
                    borderWidth: 1,
                    data: response.MRCCTV.TotalError
                },{
                    label: response.MRTF.Label,
                    backgroundColor: "rgba(3,148,71, 0.9)",
                    borderColor: "rgba(3,148,71, 0.1)",
                    borderWidth: 1,
                    data: response.MRTF.TotalError
                }, {
                    label: response.MRVMS.Label,
                    backgroundColor: "rgba(103,9,9, 0.9)",
                    borderColor: "rgba(103,9,9, 0.1)",
                    borderWidth: 1,
                    data: response.MRVMS.TotalError
                },{
                    label: response.MRWIM.Label,
                    backgroundColor: "rgba(255, 101, 47, 0.9)",
                    borderColor: "rgba(255, 101, 47, 0.1)",
                    borderWidth: 1,
                    data: response.MRWIM.TotalError
                }];
                barChartData.datasets = newDatasets;
                mybarChart.update();
            },
            error: function (status) {
                console.log(status);
            }
        });
    };

    function YearChange(value) {
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
            data: {Year: value},
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
                    backgroundColor: "rgba(3,148,71, 0.9)",
                    borderColor: "rgba(3,148,71, 0.1)",
                    borderWidth: 1,
                    data: response.MRTF.TotalError
                };

                barChartData.datasets.push(newDataset);

                newDataset = {
                    label: response.MRVMS.Label,
                    backgroundColor: "rgba(103,9,9, 0.9)",
                    borderColor: "rgba(103,9,9, 0.1)",
                    borderWidth: 1,
                    data: response.MRVMS.TotalError
                };
                barChartData.datasets.push(newDataset);

                newDataset = {
                    label: response.MRWIM.Label,
                    backgroundColor: "rgba(255, 101, 47, 0.9)",
                    borderColor: "rgba(255, 101, 47, 0.1)",
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

    

    function loadeCharts() {
        var theme = {
            color: [
                '#0900FF', '#039447', '#670909', '#ff652f',
                '#9B59B6', '#8abb6f', '#759c6a', '#bfd3b7'
            ],

            title: {
                itemGap: 8,
                textStyle: {
                    fontWeight: 'normal',
                    color: '#408829'
                }
            },

            dataRange: {
                color: ['#1f610a', '#97b58d']
            },

            toolbox: {
                color: ['#408829', '#408829', '#408829', '#408829']
            },

            tooltip: {
                backgroundColor: 'rgba(0,0,0,0.5)',
                axisPointer: {
                    type: 'line',
                    lineStyle: {
                        color: '#408829',
                        type: 'dashed'
                    },
                    crossStyle: {
                        color: '#408829'
                    },
                    shadowStyle: {
                        color: 'rgba(200,200,200,0.3)'
                    }
                }
            },

            dataZoom: {
                dataBackgroundColor: '#eee',
                fillerColor: 'rgba(64,136,41,0.2)',
                handleColor: '#408829'
            },
            grid: {
                borderWidth: 0
            },

            categoryAxis: {
                axisLine: {
                    lineStyle: {
                        color: '#408829'
                    }
                },
                splitLine: {
                    lineStyle: {
                        color: ['#eee']
                    }
                }
            },

            valueAxis: {
                axisLine: {
                    lineStyle: {
                        color: '#408829'
                    }
                },
                splitArea: {
                    show: true,
                    areaStyle: {
                        color: ['rgba(250,250,250,0.1)', 'rgba(200,200,200,0.1)']
                    }
                },
                splitLine: {
                    lineStyle: {
                        color: ['#eee']
                    }
                }
            },
            timeline: {
                lineStyle: {
                    color: '#408829'
                },
                controlStyle: {
                    normal: { color: '#408829' },
                    emphasis: { color: '#408829' }
                }
            },

            k: {
                itemStyle: {
                    normal: {
                        color: '#68a54a',
                        color0: '#a9cba2',
                        lineStyle: {
                            width: 1,
                            color: '#408829',
                            color0: '#86b379'
                        }
                    }
                }
            },
            map: {
                itemStyle: {
                    normal: {
                        areaStyle: {
                            color: '#ddd'
                        },
                        label: {
                            textStyle: {
                                color: '#c12e34'
                            }
                        }
                    },
                    emphasis: {
                        areaStyle: {
                            color: '#99d2dd'
                        },
                        label: {
                            textStyle: {
                                color: '#c12e34'
                            }
                        }
                    }
                }
            },
            force: {
                itemStyle: {
                    normal: {
                        linkStyle: {
                            strokeColor: '#408829'
                        }
                    }
                }
            },
            chord: {
                padding: 4,
                itemStyle: {
                    normal: {
                        lineStyle: {
                            width: 1,
                            color: 'rgba(128, 128, 128, 0.5)'
                        },
                        chordStyle: {
                            lineStyle: {
                                width: 1,
                                color: 'rgba(128, 128, 128, 0.5)'
                            }
                        }
                    },
                    emphasis: {
                        lineStyle: {
                            width: 1,
                            color: 'rgba(128, 128, 128, 0.5)'
                        },
                        chordStyle: {
                            lineStyle: {
                                width: 1,
                                color: 'rgba(128, 128, 128, 0.5)'
                            }
                        }
                    }
                }
            },
            gauge: {
                startAngle: 225,
                endAngle: -45,
                axisLine: {
                    show: true,
                    lineStyle: {
                        color: [[0.2, '#86b379'], [0.8, '#68a54a'], [1, '#408829']],
                        width: 8
                    }
                },
                axisTick: {
                    splitNumber: 10,
                    length: 12,
                    lineStyle: {
                        color: 'auto'
                    }
                },
                axisLabel: {
                    textStyle: {
                        color: 'auto'
                    }
                },
                splitLine: {
                    length: 18,
                    lineStyle: {
                        color: 'auto'
                    }
                },
                pointer: {
                    length: '90%',
                    color: 'auto'
                },
                title: {
                    textStyle: {
                        color: '#333'
                    }
                },
                detail: {
                    textStyle: {
                        color: 'auto'
                    }
                }
            },
            textStyle: {
                fontFamily: 'Arial, Verdana, sans-serif'
            }
        };
        if ($('#echart_pie2').length) {

            var echartPieCollapse = echarts.init(document.getElementById('echart_pie2'), theme);
        } 
        
        $.ajax({
            type: "GET",
            url: "/Report/ErrorReport",
            data: {},
            dataType: "json",
            beforeSend: function () {

            },
            success: function (response) {
      
                echartPieCollapse.setOption({
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    legend: {
                        x: 'center',
                        y: 'bottom',
                        data: ['Camera', 'Đèn tín hiệu', 'VMS', 'Trạm cân']
                    },
                    toolbox: {
                        show: true,
                        feature: {
                            magicType: {
                                show: true,
                                type: ['pie', 'funnel']
                            },
                            restore: {
                                show: true,
                                title: "Restore"
                            },
                            saveAsImage: {
                                show: true,
                                title: "Save Image"
                            }
                        }
                    },
                    calculable: true,
                    series: [{
                        name: 'Lỗi tồn tại',
                        type: 'pie',
                        radius: [25, 90],
                        center: ['50%', 120],
                        roseType: 'radius',
                        x: '0%',
                        max: 40,
                        sort: 'ascending',
                        data: [{
                            value: response.CCTVError,
                            name: 'Camera'
                        }, {
                            value:  response.TFError,
                            name: 'Đèn tín hiệu'
                        }, {
                            value: response.VMSError,
                            name: 'VMS'
                        }, {
                             value: response.WIMError,
                            name: 'Trạm cân'
                        }]
                    }]
                });

            },
            error: function (status) {
                console.log(status);
            }
        });

    };


    function registerEvents() {
        
        
    }
}