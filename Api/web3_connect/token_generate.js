import {
	Metaplex,
	keypairIdentity,
	bundlrStorage,
	toMetaplexFile,
	token,
} from '@metaplex-foundation/js';
import {
	Connection,
	clusterApiUrl,
	Keypair,
	PublicKey,
	Transaction,
} from '@solana/web3.js';
import {
	createMint,
	getOrCreateAssociatedTokenAccount,
	mintTo,
	transfer,
	createTransferInstruction,
} from '@solana/spl-token';

import * as fs from 'fs';
let num = 1;
async function SmartContract(jsonData) {
	try {
		console.log(jsonData)
		const mintAddress = new PublicKey(jsonData.mintAddress);
		console.log('Mint Address:', mintAddress.toBase58()); // Debugging: Print mint address
		const QUICKNODE_RPC =
			'https://delicate-sleek-wind.solana-devnet.discover.quiknode.pro/2bdca44cbde3506bdeb6fe4152e880d6ce4ba78f/';
		const connection = new Connection(QUICKNODE_RPC, {
			commitment: 'confirmed',
		});
		await connection
			.getVersion()
			.then((response) => console.log('Solana Version:', response)); // Debugging: Print Solana version

		let secret = [
			239, 33, 157, 225, 81, 0, 41, 217, 15, 133, 25, 166, 13, 141, 221, 89, 82,
			1, 238, 38, 170, 6, 134, 99, 178, 139, 110, 67, 247, 233, 188, 15, 37, 22,
			63, 68, 234, 130, 109, 51, 10, 249, 25, 125, 152, 249, 175, 27, 42, 234,
			147, 179, 9, 57, 83, 64, 21, 87, 168, 103, 69, 69, 31, 132,
		];
		let publicKey = '3Vmm6fgZZapv3hxeuoGhNZCY9VMj2Ykzry7jdfv2kjUP';
		const wallet = Keypair.fromSecretKey(new Uint8Array(secret));
		console.log('Wallet Public Key:', wallet.publicKey.toBase58()); // Debugging: Print wallet public key
		console.log('Public Key:', publicKey);

		const imageBuffer = fs.readFileSync('D:/Unity Projects/Saved XR 7-22/MnemonicOfLifeline/Assets/Images/p3.png');

		const file = toMetaplexFile(imageBuffer, 'p3.jpg');
		console.log('Metaplex File:', file); // Debugging: Print Metaplex file information

		const METAPLEX = Metaplex.make(connection)
			.use(keypairIdentity(wallet))
			.use(
				bundlrStorage({
					address: 'https://devnet.bundlr.network',
					providerUrl: QUICKNODE_RPC,
					timeout: 60000,
				}),
			);

		const { uri, metadata } = await METAPLEX.nfts().uploadMetadata({
			name: 'my story',
			symbol: 'Sol',
			description: 'LightBox Entertainment',
			image: file,
		});
		let NftName = 'LightHouse #' + num;
		num = num + 1;
		const { nft } = await METAPLEX.nfts().create(
			{
				uri: uri,
				name: NftName,
				sellerFeeBasisPoints: 500, // Represents 5.00%.
			},
			{ commitment: 'finalized' },
		);
		console.log(nft);
		const mint = nft.mint.address;
		console.log('Wallet Address:', wallet.publicKey);
		console.log('Wallet Address:', mintAddress);
		console.log('NFT Address:', mint); // Debugging: Print NFT address
		console.log('Token Created');
		// await METAPLEX.nfts().transfer({
		// 	nftOrSft: nftAddress,
		// 	authority: wallet,
		// 	fromOwner: publicKey,
		// 	toOwner: mintAddress,
		// 	amount: token(1),
		// });
		let commitment = 'finalized';
		const fromTokenAccount = await getOrCreateAssociatedTokenAccount(
			connection,
			wallet,
			mint,
			wallet.publicKey,
			commitment,
		);
		const toTokenAccount = await getOrCreateAssociatedTokenAccount(
			connection,
			wallet,
			mint,
			mintAddress,
			commitment,
		);

		let signature = await transfer(
			connection,
			wallet,
			fromTokenAccount.address,
			toTokenAccount.address,
			wallet.publicKey,
			1,
			[],
		);

		console.log('Token Sent');
	} catch (error) {
		console.error('Error in minting NFT:', error); // Debugging: Print detailed error message
	}
}

export default SmartContract;
