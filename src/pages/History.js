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
  MDBDataTable,
  MDBBadge,
  MDBDataTableV5,
} from "mdbreact";
import "./HomePage.css";

class History extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      data: {},
    };
  }

  refreshDta() {
    fetch("http://smartmonitoring.somee.com/api/Datatable/GetHistory")
      .then((res) => res.json())
      .then((json) => {
        let data = json[0];
        let { columns, rows } = json[0];

        rows = rows.map((row, key) => ({
          ...row,
          /*id: (
            <MDBBadge
              color="info"
              className="w-100"
              searchvalue={key}
              key={key}
            >
              {key}
            </MDBBadge>
          ),*/
        }));

        data = {
          columns,
          rows,
        };

        console.log(columns);
        console.log(rows);

        this.setState({ data });
      });
  }

  componentDidMount() {
    //window.setInterval(() => {

    this.refreshDta();
    //}, 3000);
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
                  <MDBCol md="12">
                    <MDBAnimation reveal type="fadeInLeft">
                      <MDBCard cascade className="my-3 grey lighten-4">
                        <MDBCardBody cascade className="text-center">
                          <MDBCardTitle>
                            <MDBIcon icon="cubes" className="pink-text pr-2" />
                            <strong>History</strong>
                          </MDBCardTitle>

                          {/*                         <MDBDataTable
                            striped
                            bordered
                            hover
                            data= {this.state.data}
                          />*/}

                          <MDBDataTableV5
                            hover
                            striped
                            bordered
                            entriesOptions={[5, 20, 25]}
                            entries={5}
                            pagesAmount={4}
                            data={this.state.data}
                            materialSearch
                            searchTop
                            searchBottom={false}
                          />
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

export default History;
