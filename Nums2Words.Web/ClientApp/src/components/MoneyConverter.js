import React, { Component } from 'react';

export class MoneyConverter extends Component {
  static displayName = MoneyConverter.name;

  constructor(props) {
    super(props);
    this.state = { loading: false, words: "", inputValue: "", errorMessage: "" };
  }

  renderForecastsTable() {
    return (
      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', marginTop: '20px' }}>
        <div style={{ display: 'flex', marginBottom: '20px' }}>
          <form onSubmit={this.handleInputChange}>
            <input
              type="text"
              value={this.state.inputValue}
              onChange={this.handleInputChange}
              placeholder="Enter amount to convert"
              style={{ marginRight: '10px' }}
            />
            <button className="btn btn-primary" onClick={this.convertToWords}>Convert to Words</button>
          </form>
        </div>
        {this.state.loading && <p><em>Loading...</em></p>}
        {this.state.errorMessage && <p style={{ color: 'red' }}>{this.state.errorMessage}</p>}
        <p>{this.state.words}</p>
      </div>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderForecastsTable();

    return (
      <div>
        {contents}
      </div>
    );
  }

  handleInputChange = (e) => {
    this.setState({ inputValue: e.target.value, errorMessage: "" });
  }

  validateInput = (input) => {
    const regex = /^-?(\d{1,3}( \d{3})*|\d+)(,\d{1,2})?$/;
    return regex.test(input);
  }

  convertToWords = async (e) => {
    e.preventDefault();
    
    if (!this.validateInput(this.state.inputValue)) {
      this.setState({ words: "", errorMessage: "Invalid number format. Please enter a number in the following format: -999 999 999,99. The number can be up to 999 999 999,99, negative numbers are allowed, spaces between groups of three digits are optional, and the fractional part separated by a comma is optional but should not exceed two decimal places." });
      return;
    }

    this.setState({ loading: true });

    try {
      const response = await fetch('api/conversions', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ amount: this.state.inputValue })
      });

      if (response.ok) {
        const data = await response.json();
        this.setState({ loading: false, words: data.result });
      } else {
        const error = await response.text();

        try {
          const errorJSON = JSON.parse(error);
          this.setState({ loading: false, errorMessage: errorJSON.errors.Amount[0], words: "" });
        } catch (error) {
          this.setState({ loading: false, errorMessage: error, words: "" });
        }

      }
    } catch (error) {
      this.setState({ errorMessage: "An error occurred while converting the number.", loading: false, words: "" });

    }
  }
}
