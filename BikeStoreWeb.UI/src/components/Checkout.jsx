import { useState } from 'react';
import { Container, Row, Col, Form, Button, Card, ListGroup, Alert } from "react-bootstrap";
import { useCart } from "../context/CartContext";
import api from "../services/api";
import { useNavigate } from 'react-router-dom';

const Checkout = () => {
    const { cartItems, cartTotal, clearCart } = useCart();
    const navigate = useNavigate();

    //Adres bilgileri için state
    const [address, setAddress] = useState({
        street: '',
        city: '',
        zipCode: '',
        country: 'Türkiye'
    });

    const [error, setError] = useState(null);
    //form elemanları değiştiğinde state'i güncelle
    const handleChange = (e) => {
        setAddress({
            ...address, [e.target.name]: e.target.value
        });
    };

    //Siparişi gönder
    const handlePlaceOrder = async (e) => {
        e.preventDefault();
        setError(null);

        const chechoutDto = {
            shippingAddress: `${address.street}, ${address.city}, ${address.zipCode}, ${address.country}`,
            orderItems: cartItems.map(item => ({
                productId: item.id,
                productName: item.name,
                quantity: item.quantity,
                price: item.price
            })),
        };
        console.log("🚀 GÖNDERİLEN PAKET:", JSON.stringify(chechoutDto, null, 2)); // 
        try {
            //backend'e post isteği gönder
            const response = await api.post('/orders/checkout', chechoutDto);
            if (response.status === 200 || response.data.success) {
                alert(" Siparişiniz başarıyla alındı!");
                clearCart();    // Sepeti temizle
                navigate('/');  // Anasayfaya yönlendir
            }

        } catch (err) {
            console.error("Sipariş gönderilirken hata oluştu:", err);
            const errorMessage = err.response?.data?.message || "Sipariş gönderilirken bir hata oluştu.";
            setError(errorMessage);
        }
    };

    // Eğer sepet boşsa buraya girme, ana sayfaya dön
    if (cartItems.length === 0) {
        return <div className="text-center mt-5">Sepetiniz boş, ödeme yapamazsınız.</div>;
    }


    return (
        <Container className="mt-5">
            <h2 className="mb-4 text-center">📦 Siparişi Tamamla</h2>
            {error && <Alert variant="danger">{error}</Alert>}

            <Row>
                {/* SOL TARAF: ADRES FORMU */}
                <Col md={7}>
                    <Card className="shadow-sm p-4 mb-4">
                        <h4 className="mb-3">Teslimat Adresi</h4>
                        <Form onSubmit={handlePlaceOrder}>
                            <Form.Group className="mb-3">
                                <Form.Label>Adres (Cadde/Sokak/No)</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="street"
                                    required
                                    onChange={handleChange}
                                    placeholder="Örn: Atatürk Cad. No:5"
                                />
                            </Form.Group>

                            <Row>
                                <Col md={6}>
                                    <Form.Group className="mb-3">
                                        <Form.Label>Şehir</Form.Label>
                                        <Form.Control
                                            type="text"
                                            name="city"
                                            required
                                            onChange={handleChange}
                                            placeholder="İstanbul"
                                        />
                                    </Form.Group>
                                </Col>
                                <Col md={6}>
                                    <Form.Group className="mb-3">
                                        <Form.Label>Posta Kodu</Form.Label>
                                        <Form.Control
                                            type="text"
                                            name="zipCode"
                                            required
                                            onChange={handleChange}
                                            placeholder="34000"
                                        />
                                    </Form.Group>
                                </Col>
                            </Row>

                            <Form.Group className="mb-3">
                                <Form.Label>Ülke</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="country"
                                    value={address.country}
                                    readOnly
                                />
                            </Form.Group>
                        </Form>
                    </Card>
                </Col>

                {/* SAĞ TARAF: ÖZET */}
                <Col md={5}>
                    <Card className="shadow-sm">
                        <Card.Header className="bg-primary text-white">Sipariş Özeti</Card.Header>
                        <ListGroup variant="flush">
                            {cartItems.map((item) => (
                                <ListGroup.Item key={item.id} className="d-flex justify-content-between align-items-center">
                                    <div>
                                        <span className="fw-bold">{item.name}</span> <br />
                                        <small className="text-muted">{item.quantity} x ${item.price}</small>
                                    </div>
                                    <span>${(item.price * item.quantity).toFixed(2)}</span>
                                </ListGroup.Item>
                            ))}
                            <ListGroup.Item className="d-flex justify-content-between fw-bold bg-light">
                                <span>TOPLAM TUTAR</span>
                                <span className="text-success fs-5">${cartTotal.toFixed(2)}</span>
                            </ListGroup.Item>
                        </ListGroup>
                        <Card.Body>
                            <Button variant="success" size="lg" className="w-100" onClick={handlePlaceOrder}>
                                ✅ Siparişi Onayla
                            </Button>
                        </Card.Body>
                    </Card>
                </Col>
            </Row>
        </Container>
    );

};

export default Checkout;
