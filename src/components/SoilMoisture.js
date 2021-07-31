import ReactApexChart from "react-apexcharts";
import React, { Component } from "react";

class SoilMoisture extends Component {
  constructor(props) {
    super(props);

    this.state = {
      series: [0],
      options: {
        chart: {
          height: 350,
          type: "radialBar",
        },
        plotOptions: {
          radialBar: {
            hollow: {
              size: "50%",
            },
            dataLabels: {
              show: true,
              name: {
                show: true,
                fontSize: "16px",
                fontFamily: undefined,
                fontWeight: 600,
                color: undefined,
                offsetY: -10,
              },
              value: {
                show: true,
                fontSize: "14px",
                fontFamily: undefined,
                fontWeight: 400,
                color: undefined,
                offsetY: 16,
                formatter: function (val) {
                  return val + "%";
                },
              },
            },
          },
        },
        labels: ["Soil Moisture"],
      },
    };
  }

  componentDidMount() {
    //Set update interval
    window.setInterval(() => {
      fetch("http://www.smartmonitoring.somee.com/api/Data/GetLastData")
        .then((res) => res.json())
        .then((resJson) => {
          this.setState({
            series: [parseInt(Math.round(resJson[0].soilMoisture, 0))],
          });
        })
        .catch((e) => {
          console.log("Error : " + e);
        });
    }, 3000);
  }

  render() {
    return (
      <div id="chart3">
        <ReactApexChart
          options={this.state.options}
          series={this.state.series}
          type="radialBar"
          height={350}
        />
      </div>
    );
  }
}

export default SoilMoisture;
