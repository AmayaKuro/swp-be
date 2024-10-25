exports.config = {
  tests: './Controllers/Test/*_test.js',
  plugins: {
    allure: {
      enabled: true,
      require: "allure-codeceptjs",
    },
  },
  output: './output',
  helpers: {
    REST: {
      endpoint: 'http://localhost:5213', // Adjust this to your API's base URL
      defaultHeaders: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }
    },
    JSONResponse: {}
  },
  name: 'koi-api-tests'
}
