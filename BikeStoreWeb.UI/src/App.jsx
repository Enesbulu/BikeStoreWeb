import ProductList from './components/ProductList';

import './App.css'

function App() {
  return (
    <div className="container mt-5">
      <h1 className="text-center mb-4 text-primary">🚲 BikeStore Ürünleri</h1>

      {/* Ürün Listesi Bileşeni */}
      <ProductList />

    </div>
  )


}

export default App
