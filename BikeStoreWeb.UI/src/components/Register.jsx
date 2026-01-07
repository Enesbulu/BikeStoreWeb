import { useState } from "react";
import { Container, Form, Button, Card, Alert } from "react-bootstrap";
import api from '../services/api';
import { useNavigate } from 'react-router-dom';

const Register = () => {
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        confirmPassword: '',
        firstName: '',
        lastName: '',

    });

    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError(null);

        if (formData.password !== formData.confirmPassword) {
            setError("Şifreler eşleşmiyor!")
            return;
        }


        //backend Register isteği
        try {
            const response = await api.post('/auth/register', {
                email: formData.email,
                password: formData.password,
                confirmPassword: formData.confirmPassword,
                firstName: formData.firstName,
                lastName: formData.lastName,
                username: formData.email    //username yerine mail adresi kullnıyoruz.
            });

            if (response.data.success) {
                alert("Kayıt Başarılı! şimdi giriş Yapabilirisiniz..");
                navigate('/login');
            }
        } catch (err) {
            setError(err.response?.data?.message || "Kayıt işlemi başarısız.");
        };

    };

    return (
        <Container className="d-flex justify-content-center align-items-center mt-5">
            <Card style={{ width: '500px' }} className="shadow">
                <Card.Body>
                    <h2 className="text-center mb-4">Kayıt Ol</h2>
                    {error && <Alert variant="danger">{error}</Alert>}

                    <Form onSubmit={handleSubmit}>
                        <Form.Group className="mb-3">
                            <Form.Label>Ad</Form.Label>
                            <Form.Control name="firstName" onChange={handleChange} required />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Soyad</Form.Label>
                            <Form.Control name="lastName" onChange={handleChange} required />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Email</Form.Label>
                            <Form.Control type="email" name="email" onChange={handleChange} required />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Şifre</Form.Label>
                            <Form.Control type="password" name="password" onChange={handleChange} required />
                        </Form.Group>
                        <Form.Group className="mb-3">
                            <Form.Label>Şifre Tekrar</Form.Label>
                            <Form.Control type="password" name="confirmPassword" onChange={handleChange} required />
                        </Form.Group>

                        <Button variant="success" type="submit" className="w-100">
                            Kayıt Ol
                        </Button>
                    </Form>
                </Card.Body>
            </Card>
        </Container>
    );
};

export default Register;