import { Container, Table, Button, Alert, Card, Row, Col } from 'react-bootstrap';
import { useCart } from '../context/CartContext';
import { Link, useNavigate } from 'react-router-dom';

const Cart = () => {
    // Kullanılmayan handleDecrease'i sildik, hatalar gitti.
    const { cartItems, addToCart, removeFromCart, clearCart, cartTotal } = useCart();
    const navigate = useNavigate();

    // Sepet Boşsa
    if (cartItems.length === 0) {
        return (
            <Container className="mt-5 text-center">
                <Alert variant="info">
                    <h4>Sepetinizde ürün bulunmamaktadır.</h4>
                    <p>Hemen alışverişe başlayın!</p>
                    <Button as={Link} to="/" variant="primary">Ürünlere Git</Button>
                </Alert>
            </Container>
        );
    }

    return (
        <Container className="mt-5">
            <h2 className="mb-4">🛒 Alışveriş Sepetim</h2>

            <Row>
                <Col md={8}>
                    <Table responsive hover className="shadow-sm">
                        <thead className="bg-light">
                            <tr>
                                <th>Ürün</th>
                                <th>Fiyat</th>
                                <th>Adet</th>
                                <th>Toplam</th>
                                <th>İşlem</th>
                            </tr>
                        </thead>
                        <tbody>
                            {cartItems.map((item) => (
                                <tr key={item.id} className="align-middle">
                                    <td>
                                        <div className="d-flex align-items-center">
                                            <img
                                                src={item.imageUrl || "https://placehold.co/100"}
                                                alt={item.name}
                                                style={{ width: '50px', height: '50px', objectFit: 'cover', marginRight: '10px', borderRadius: '5px' }}
                                            />
                                            <span>{item.name}</span>
                                        </div>
                                    </td>
                                    <td>${item.price}</td>
                                    <td>
                                        {/* Artık addToCart kullanıldığı için hata vermeyecek */}
                                        <div className="d-flex align-items-center">
                                            <span className="fw-bold mx-2 fs-5">{item.quantity}</span>
                                            <Button
                                                variant="outline-secondary"
                                                size="sm"
                                                className="rounded-circle"
                                                onClick={() => addToCart(item)}
                                            >
                                                +
                                            </Button>
                                        </div>
                                    </td>
                                    <td className="fw-bold">${(item.price * item.quantity).toFixed(2)}</td>
                                    <td>
                                        <Button variant="danger" size="sm" onClick={() => removeFromCart(item.id)}>
                                            🗑️ Sil
                                        </Button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </Table>

                    <Button variant="outline-danger" size="sm" onClick={clearCart}>
                        Sepeti Temizle
                    </Button>
                </Col>

                <Col md={4}>
                    <Card className="shadow-sm border-0">
                        <Card.Body>
                            <Card.Title className="mb-4">Sipariş Özeti</Card.Title>
                            <div className="d-flex justify-content-between mb-2">
                                <span>Ara Toplam:</span>
                                <span>${cartTotal.toFixed(2)}</span>
                            </div>
                            <div className="d-flex justify-content-between mb-3">
                                <span>Kargo:</span>
                                <span className="text-success">Ücretsiz</span>
                            </div>
                            <hr />
                            <div className="d-flex justify-content-between mb-4 fw-bold fs-5">
                                <span>Toplam:</span>
                                <span>${cartTotal.toFixed(2)}</span>
                            </div>

                            {/* Artık navigate kullanıldığı için hata vermeyecek */}
                            <Button
                                variant="success"
                                size="lg"
                                className="w-100"
                                onClick={() => navigate('/checkout')}
                            >
                                Siparişi Tamamla
                            </Button>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </Container>
    );
};

export default Cart;