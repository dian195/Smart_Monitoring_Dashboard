import React from "react";
import {
  MDBEdgeHeader,
  MDBFreeBird,
  MDBContainer,
  MDBCol,
  MDBRow,
  MDBCardBody,
  MDBIcon,
  MDBCard,
  MDBCardTitle,
  MDBCardImage,
  MDBCardText,
  MDBAnimation,
  MDBNavLink,
} from "mdbreact";
import "./HomePage.css";
import TemperatureComp from "../components/Temperature";
import HumidityComp from "../components/Humidity";
import SoilMoistureComp from "../components/SoilMoisture";

class HomePage extends React.Component {

    constructor(props) {
        super(props);
    
        this.state = {
          statusData:'None'
        };
      }
    
      componentDidMount() { //Set update interval
        window.setInterval(() => {
    
          fetch('http://www.smartmonitoring.somee.com/api/Data/GetLastData')
                .then(res => res.json())
                .then(resJson => {
                  this.setState({ statusData: resJson[0].status });
                }).catch(e => 
                    {
                        console.log('Error : ' + e);
                    });
        }, 3000)
      }

  scrollToTop = () => window.scrollTo(0, 0);

  render() {
    return (
      <>
        <MDBEdgeHeader color="indigo darken-3" className="sectionPage" />
        <div className="mt-3 mb-5">
          <MDBContainer>
            <MDBRow>
              <MDBCol md="12" className="mt-4">
                
                <MDBRow id="categories">
                  <MDBCol md="4">
                    <MDBAnimation reveal type="fadeInLeft">
                      <MDBCard cascade className="my-3 grey lighten-4">
                        <MDBCardBody cascade className="text-center">
                          <MDBCardTitle>
                            <MDBIcon
                              icon="cubes"
                              
                              className="pink-text pr-2"
                            />
                            <strong>Temperature</strong>
                          </MDBCardTitle>
                          
                          <TemperatureComp />

                        </MDBCardBody>
                      </MDBCard>
                    </MDBAnimation>
                  </MDBCol>
                  <MDBCol md="4">
                    <MDBAnimation reveal type="fadeInDown">
                      <MDBCard cascade className="my-3 grey lighten-4">
                        <MDBCardBody cascade className="text-center">
                          <MDBCardTitle>
                            <MDBIcon icon="cubes" className="blue-text pr-2" />
                            <strong>Humidity</strong>
                          </MDBCardTitle>
                          <HumidityComp />
                        </MDBCardBody>
                      </MDBCard>
                    </MDBAnimation>
                  </MDBCol>
                  <MDBCol md="4">
                    <MDBAnimation reveal type="fadeInRight">
                      <MDBCard cascade className="my-3 grey lighten-4">
                        <MDBCardBody cascade className="text-center">
                          <MDBCardTitle>
                            <MDBIcon icon="cubes" className="green-text pr-2" />
                            <strong>Soil Moisture</strong>
                          </MDBCardTitle>
                          <SoilMoistureComp />
                        </MDBCardBody>
                      </MDBCard>
                    </MDBAnimation>
                  </MDBCol>
                </MDBRow>

                <MDBRow id="categories">
                  <MDBCol md="12">
                    <MDBAnimation reveal type="fadeInLeft">
                      <MDBCard cascade className="my-3 grey lighten-4">
                        
                        <MDBCardBody cascade className="text-center">
                            <MDBIcon icon="angle-double-right" className="pink-text pr-2" />
                            <strong>{ this.state.statusData }</strong>
                            <MDBIcon icon="angle-double-left" className="pink-text pl-2" />
                          
                        </MDBCardBody>
                      </MDBCard>
                    </MDBAnimation>
                  </MDBCol>
                </MDBRow>
              </MDBCol>
            </MDBRow>
          </MDBContainer>
        </div>
      </>
    );
  }
}

export default HomePage;
