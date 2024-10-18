Feature('Koi API');

let createdKoiId;

Scenario('Create and retrieve koi', async ({ I }) => {
  // GET all kois
  const getAllResponse = await I.sendGetRequest('/api/koi/Koi');
  I.seeResponseCodeIsSuccessful();
  I.seeResponseContainsKeys(['koiID', 'name', 'color']);

  // POST a new koi
  const newKoi = {
    name: 'Test Koi',
    color: 'Red and White',
    age: 2,
    gender: 'Female',
    size: 'Medium',
    price: 1000,
    species: 'Kohaku'
  };
  const createResponse = await I.sendPostRequest('/api/koi/Koi', newKoi);
  I.seeResponseCodeIs(201);
  I.seeResponseContainsJson(newKoi);
  
  // Store the created koi ID for later use
  createdKoiId = createResponse.data.koiID;
});

Scenario('Update a koi', async ({ I }) => {
  const updatedKoi = {
    name: 'Updated Test Koi',
    color: 'Black and White',
    age: 3,
    gender: 'Male',
    size: 'Large',
    price: 1500,
    species: 'Showa'
  };

  // PUT request to update the koi
  const updateResponse = await I.sendPutRequest(`/api/koi/Koi/${createdKoiId}`, updatedKoi);
  I.seeResponseCodeIs(200);
  I.seeResponseContainsJson(updatedKoi);

  // GET the updated koi to verify changes
  const getUpdatedResponse = await I.sendGetRequest(`/api/koi/Koi/${createdKoiId}`);
  I.seeResponseCodeIs(200);
  I.seeResponseContainsJson(updatedKoi);
});

Scenario('Get a specific koi', async ({ I }) => {
  // GET request for a specific koi
  const getSpecificResponse = await I.sendGetRequest(`/api/koi/Koi/${createdKoiId}`);
  I.seeResponseCodeIs(200);
  I.seeResponseContainsKeys(['koiID', 'name', 'color', 'age', 'gender', 'size', 'price', 'species']);
});

Scenario('Delete a koi', async ({ I }) => {
  // DELETE request to remove the koi
  const deleteResponse = await I.sendDeleteRequest(`/api/koi/Koi/${createdKoiId}`);
  I.seeResponseCodeIs(204);

  // Attempt to GET the deleted koi (should return 404)
  const getDeletedResponse = await I.sendGetRequest(`/api/koi/Koi/${createdKoiId}`);
  I.seeResponseCodeIs(404);
});

Scenario('Attempt to create an invalid koi', async ({ I }) => {
  const invalidKoi = {
    name: '', // Invalid: empty name
    color: 'Purple', // Assuming purple is not a valid koi color
    age: -1, // Invalid: negative age
    gender: 'Unknown', // Invalid: not Male or Female
    size: 'Enormous', // Invalid: not a valid size
    price: 'Free', // Invalid: not a number
    species: 'Dragon' // Invalid: not a real koi species
  };

  const createInvalidResponse = await I.sendPostRequest('/api/koi/Koi', invalidKoi);
  I.seeResponseCodeIs(400); // Bad Request
  I.seeResponseValidationErrorsContains(['name', 'color', 'age', 'gender', 'size', 'price', 'species']);
});
