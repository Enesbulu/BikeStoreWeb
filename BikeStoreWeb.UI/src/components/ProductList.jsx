import { useEffect, useState } from 'react';
import api from '../services/api';

const ProductList = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        // Backend'den ürünleri çek
        const fetchProducts = async () => {
            try {
                // GET /products endpoint'ine istek at
                const response = await api.get('/products');
                // Backend'den gelen veri yapısına göre (ServiceResponse.Data)
                // Eğer response.data.data ise:
                setProducts(response.data.data);
            } catch (err) {
                console.error("Hata:", err);
                setError("Ürünler yüklenirken bir hata oluştu.");
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    if (loading) return <div className="text-center mt-5">Yükleniyor...</div>;
    if (error) return <div className="alert alert-danger">{error}</div>;

    return (
        <div className="row">
            {products.map((product) => (
                <div key={product.id} className="col-md-4 mb-4">
                    <div className="card h-100 shadow-sm">
                        <div className="card-body">
                            <h5 className="card-title">{product.name}</h5>
                            <p className="card-text">{product.description}</p>
                            <h6 className="text-primary">${product.price}</h6>
                            <button className="btn btn-sm btn-outline-primary w-100">Sepete Ekle</button>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default ProductList;