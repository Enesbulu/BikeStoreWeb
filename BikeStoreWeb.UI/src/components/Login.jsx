import { useState } from "react";
import { Container, Form, Button, Card, Alert } from "react-bootstrap";
import api from '../services/api';
import { useNavigate } from 'react-router-dom';




const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(null);
    const navigate = useNavigate(); //sayfa değiştirmek için kullanılır.

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        try {
            //backend login isteği
            const response = await api.post('/auth/login', {
                email: email,
                password: password
            });

            if (response.data.success) {
                //Token kaydetme
                localStorage.setItem('token', response.data.data.token);
                localStorage.setItem('userEmail', response.data.data.email);

                //Anasayfaya yönlendirme
                alert("Giriş Başarılı!")
                navigate('/');

                //Navbar güncellenenmesi için sayfayı yenileme
                window.location.reload();
            }
        } catch (error) {
            setError("Giriş Başarısız. Email veya Şifre Hatalı");
            console.error(error);
        }
    };

    return (
        <Container className="d-flex justify-content-center align-items-center" style={{ minHeight: "80vh" }}>
            <Card style={{ width: '400px' }} className="shadow">
                <Card.Body>
                    <h2 className="text-center mb-4">Giriş Yap</h2>
                    {error && <Alert variant="danger">{error}</Alert>}

                    <Form onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>Email Adresi</Form.Label>
                            <Form.Control
                                type="email"
                                placeholder="ornek@email.com"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                            />
                        </Form.Group>

                        <Form.Group className="mb-3">
                            <Form.Label>Şifre</Form.Label>
                            <Form.Control
                                type="password"
                                placeholder="******"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                        </Form.Group>

                        <Button variant="primary" type="submit" className="w-100">
                            Giriş Yap
                        </Button>
                    </Form>
                </Card.Body>
            </Card>
        </Container>
    );
};

export default Login;