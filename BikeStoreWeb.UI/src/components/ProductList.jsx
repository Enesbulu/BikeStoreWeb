// src/components/ProductList.jsx
import { useEffect, useState } from 'react';
import api from '../services/api';
// Bootstrap bileşenlerini import ediyoruz
import { Row, Col, Card, Button, Spinner, Alert, Badge } from 'react-bootstrap';
import { useCart } from '../context/CartContext';

const ProductList = () => {
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const { addToCart } = useCart();

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await api.get('/products');
                // Backend'in yapısına göre data.data veya direkt data olabilir,
                // Senin yapına göre response.data.data doğruydu.
                setProducts(response.data.data);
            } catch (err) {
                console.error("Hata:", err);
                setError("Ürünler yüklenirken sunucu ile bağlantı kurulamadı.");
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    // Yükleniyor Ekranı
    if (loading) return (
        <div className="text-center mt-5">
            <Spinner animation="border" variant="primary" />
            <p className="mt-2">Bisikletler getiriliyor...</p>
        </div>
    );

    // Hata Ekranı
    if (error) return <Alert variant="danger" className="text-center">{error}</Alert>;

    return (
        <>
            <h2 className="text-center mb-4 text-dark fw-bold">🚲 Öne Çıkan Ürünler</h2>
            <Row>
                {/* map fonksiyonu ürünleri tek tek döner */}
                {products.map((product) => (
                    <Col key={product.id} sm={12} md={6} lg={4} className="mb-4">
                        <Card className="h-100 shadow border-0 product-card">
                            {/* Resim alanı (Şimdilik placeholder resim) */}
                            <Card.Img
                                variant="top"
                                src={product.imageUrl || "https://placehold.co/600x400/EEE/31343C?text=BikeStore"}
                                style={{ height: '200px', objectFit: 'cover' }}
                            />
                            <Card.Body className="d-flex flex-column">
                                <div className="d-flex justify-content-between align-items-start mb-2">
                                    <Card.Title>{product.name}</Card.Title>
                                    <Badge bg="success">${product.price}</Badge>
                                </div>

                                <Card.Text className="text-muted flex-grow-1">
                                    {product.description.substring(0, 100)}...
                                </Card.Text>

                                <div className="d-grid gap-2">
                                    <Button variant="primary" onClick={() => addToCart(product)}>
                                        Sepete Ekle 🛒
                                    </Button>
                                </div>
                            </Card.Body>
                        </Card>
                    </Col>
                ))}
            </Row>
        </>
    );
};

export default ProductList;