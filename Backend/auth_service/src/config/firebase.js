const admin = require('firebase-admin');
const { initializeApp } = require('firebase/app');
const { getAuth } = require('firebase/auth');
require('dotenv').config();

// Initialize Firebase Admin (for backend operations: verify tokens, create users without logging in)
let adminApp;
try {
  adminApp = admin.initializeApp({
    credential: admin.credential.cert({
      projectId: process.env.FIREBASE_PROJECT_ID,
      clientEmail: process.env.FIREBASE_CLIENT_EMAIL,
      privateKey: process.env.FIREBASE_PRIVATE_KEY ? process.env.FIREBASE_PRIVATE_KEY.replace(/\\n/g, '\n') : undefined,
    })
  });
  console.log('Firebase Admin initialized successfully.');
} catch (error) {
  console.error('Firebase Admin initialization error:', error.message);
  // We don't throw here to allow the server to start even if config is missing, 
  // but operations will fail later.
}

// Initialize Firebase Client (for simulating client login on the backend if needed)
const firebaseConfig = {
  apiKey: process.env.FIREBASE_API_KEY,
  authDomain: process.env.FIREBASE_AUTH_DOMAIN,
  projectId: process.env.FIREBASE_PROJECT_ID_CLIENT,
  storageBucket: process.env.FIREBASE_STORAGE_BUCKET,
  messagingSenderId: process.env.FIREBASE_MESSAGING_SENDER_ID,
  appId: process.env.FIREBASE_APP_ID
};

let clientAuth = null;
try {
  const clientApp = initializeApp(firebaseConfig);
  clientAuth = getAuth(clientApp);
  console.log('Firebase Client initialized successfully.');
} catch (error) {
  console.error('Firebase Client initialization error:', error.message);
}

module.exports = {
  admin,
  adminAuth: adminApp ? admin.auth() : null,
  clientAuth
};
