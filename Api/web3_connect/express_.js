import express from 'express';
import SmartContract from './token_generate.js';
const app = express();
const port = 3000;

// Middleware to parse JSON requests
app.use(express.json());

// Define a route for receiving JSON data
app.post('/json-data', async (req, res) => {
	try {
		console.log('Token taken');
		const jsonData = req.body; // This will contain the parsed JSON data
		await SmartContract(jsonData);
		res.send('NFT minted successfully!!');
	} catch {
		res.send('Error in minting NFT');
	}
});

// Start the server
app.listen(port, () => {
	console.log(`Server is listening on port ${port}`);
});
