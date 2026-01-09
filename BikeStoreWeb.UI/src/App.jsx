import './App.css'
import { Routes, Route } from 'react-router-dom';
import ProductList from './components/ProductList';
import CustomNavbar from './components/CustomNavbar';
import Footer from './components/Footer';
import Login from './components/Login';
import Register from './components/Register';
import Cart from './components/Cart';
import Checkout from './components/Checkout';


function App() {
  return (
    <div className='d-flex flex-column min-vh-100'>
      {/*Üst MEnü */}
      <CustomNavbar />

      {/*Gövde */}
      <main className='flex-grow-1'>
        <div className="container mt-4">
          <Routes>
            {/*Ürün Listesi */}
            <Route path="/" element={<ProductList />} />

            {/*Giriş Sayfası */}
            <Route path="/login" element={<Login />} />

            {/*Kayıt SAyfası*/}
            <Route path="/register" element={<Register />} />

            {/*Sepet Sayfası*/}
            <Route path="/cart" element={<Cart />} />

            {/*Sipariş Sayfası*/}
            <Route path="/checkout" element={<Checkout />} />

          </Routes>
        </div>
      </main>

      {/*Alt Bilgi - Footer*/}
      <Footer />

    </div>
  )
}

export default App
