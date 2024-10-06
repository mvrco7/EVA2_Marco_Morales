Imports MySql.Data.MySqlClient

Public Class Form1

    Dim connectionString As String = "Server=localhost;Port=3307;Database=registropersonas;User ID='root';Password='';"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarComunas()
        btnActualizar.Enabled = False ' Deshabilitar el botón "Actualizar" al cargar el formulario
        btnEliminar.Enabled = False   ' Deshabilitar el botón "Eliminar" al cargar el formulario
        btnGuardar.Enabled = False     ' Deshabilitar el botón "Guardar" al cargar el formulario
    End Sub

    Private Sub CargarComunas()
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()
                Dim sql As String = "select NombreComuna from comuna"

                Using cmd As New MySqlCommand(sql, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        cboComuna.Items.Clear()
                        While reader.Read()
                            cboComuna.Items.Add(reader("NombreComuna").ToString())
                        End While
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error al cargar las comunas: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validación de los campos antes de guardar
        If Not ValidarCampos() Then
            Return
        End If

        Dim rut As String = txtRut.Text
        Dim nombre As String = txtNombre.Text
        Dim apellido As String = txtApellido.Text
        Dim sexo As String

        If rbtnMasculino.Checked Then
            sexo = "Masculino"
        ElseIf rbtnFemenino.Checked Then
            sexo = "Femenino"
        ElseIf rbtnNoEspecifica.Checked Then
            sexo = "No especifica"
        End If

        Dim comuna As String = cboComuna.SelectedItem?.ToString()
        Dim ciudad As String = txtCiudad.Text
        Dim observacion As String = txtObservacion.Text

        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Dim sql As String = "insert into personas (RUT, Nombre, Apellido, Sexo, Comuna, Ciudad, Observacion) " &
                                    "values (@rut, @nombre, @apellido, @sexo, @comuna, @ciudad, @observacion)"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@rut", rut)
                    cmd.Parameters.AddWithValue("@nombre", nombre)
                    cmd.Parameters.AddWithValue("@apellido", apellido)
                    cmd.Parameters.AddWithValue("@sexo", sexo)
                    cmd.Parameters.AddWithValue("@comuna", comuna)
                    cmd.Parameters.AddWithValue("@ciudad", ciudad)
                    cmd.Parameters.AddWithValue("@observacion", observacion)

                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Datos guardados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    LimpiarFormulario()
                End Using

            Catch ex As Exception
                MessageBox.Show("Error al guardar los datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Function ValidarCampos() As Boolean
        ' Validar que todos los campos estén completos
        If String.IsNullOrWhiteSpace(txtRut.Text) Then
            MessageBox.Show("Por favor, ingrese el RUT.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtRut.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("Por favor, ingrese el Nombre.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNombre.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtApellido.Text) Then
            MessageBox.Show("Por favor, ingrese el Apellido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtApellido.Focus()
            Return False
        End If

        If Not (rbtnMasculino.Checked Or rbtnFemenino.Checked Or rbtnNoEspecifica.Checked) Then
            MessageBox.Show("Por favor, seleccione el sexo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If cboComuna.SelectedIndex = -1 Then
            MessageBox.Show("Por favor, seleccione una Comuna.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboComuna.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtCiudad.Text) Then
            MessageBox.Show("Por favor, ingrese la Ciudad.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtCiudad.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtObservacion.Text) Then
            MessageBox.Show("Por favor, ingrese una Observación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtObservacion.Focus()
            Return False
        End If

        Return True
    End Function

    Private Sub LimpiarFormulario()
        txtRut.Clear()
        txtNombre.Clear()
        txtApellido.Clear()
        txtCiudad.Clear()
        txtObservacion.Clear()
        rbtnMasculino.Checked = False
        rbtnFemenino.Checked = False
        rbtnNoEspecifica.Checked = False
        cboComuna.SelectedIndex = -1
        txtRut.Focus()
        btnActualizar.Enabled = False ' Deshabilitar el botón "Actualizar"
        btnEliminar.Enabled = False   ' Deshabilitar el botón "Eliminar"
        btnGuardar.Enabled = False    ' Deshabilitar el botón "Guardar"
    End Sub

    Private Sub LimpiarFormularios()
        txtNombre.Clear()
        txtApellido.Clear()
        txtCiudad.Clear()
        txtObservacion.Clear()
        rbtnMasculino.Checked = False
        rbtnFemenino.Checked = False
        rbtnNoEspecifica.Checked = False
        cboComuna.SelectedIndex = -1
        txtRut.Focus()
        btnActualizar.Enabled = False ' Deshabilitar el botón "Actualizar"
        btnEliminar.Enabled = False   ' Deshabilitar el botón "Eliminar"
        btnGuardar.Enabled = False    ' Deshabilitar el botón "Guardar"
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim rut As String = txtRut.Text

        If String.IsNullOrWhiteSpace(rut) Then
            MessageBox.Show("Por favor, ingrese el RUT.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Dim sql As String = "select * from personas where rut = @rut"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@rut", rut)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' Mostrar los datos encontrados
                            txtNombre.Text = reader("Nombre").ToString()
                            txtApellido.Text = reader("Apellido").ToString()
                            Dim sexo As String = reader("Sexo").ToString()

                            Select Case sexo
                                Case "Masculino"
                                    rbtnMasculino.Checked = True
                                Case "Femenino"
                                    rbtnFemenino.Checked = True
                                Case "No especifica"
                                    rbtnNoEspecifica.Checked = True
                            End Select

                            cboComuna.SelectedItem = reader("Comuna").ToString()
                            txtCiudad.Text = reader("Ciudad").ToString()
                            txtObservacion.Text = reader("Observacion").ToString()

                            ' Habilitar los botones "Actualizar" y "Eliminar" después de buscar un registro
                            btnActualizar.Enabled = True
                            btnEliminar.Enabled = True
                            MessageBox.Show("Registro encontrado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            ' Si no se encuentra el registro, preguntar si se desea agregar un nuevo usuario
                            Dim result As DialogResult = MessageBox.Show("No se encontró ningún registro con el RUT ingresado. ¿Desea agregar un nuevo usuario?", "Usuario no encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                            If result = DialogResult.Yes Then
                                LimpiarFormularios()

                                ' Habilitar controles para nuevo ingreso
                                txtNombre.Enabled = True
                                txtApellido.Enabled = True
                                rbtnMasculino.Enabled = True
                                rbtnFemenino.Enabled = True
                                rbtnNoEspecifica.Enabled = True
                                cboComuna.Enabled = True
                                txtCiudad.Enabled = True
                                txtObservacion.Enabled = True

                                btnGuardar.Enabled = True
                                txtNombre.Focus()
                            End If
                        End If
                    End Using
                End Using

            Catch ex As Exception
                MessageBox.Show("Error al buscar el registro: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim rut As String = txtRut.Text

        If String.IsNullOrWhiteSpace(rut) Then
            MessageBox.Show("Por favor, ingrese el RUT.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validación de los campos antes de actualizar
        If Not ValidarCampos() Then
            Return
        End If

        Dim nombre As String = txtNombre.Text
        Dim apellido As String = txtApellido.Text
        Dim sexo As String

        If rbtnMasculino.Checked Then
            sexo = "Masculino"
        ElseIf rbtnFemenino.Checked Then
            sexo = "Femenino"
        ElseIf rbtnNoEspecifica.Checked Then
            sexo = "No especifica"
        End If

        Dim comuna As String = cboComuna.SelectedItem?.ToString()
        Dim ciudad As String = txtCiudad.Text
        Dim observacion As String = txtObservacion.Text

        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Dim sql As String = "update personas set Nombre = @nombre, Apellido = @apellido, Sexo = @sexo, Comuna = @comuna, Ciudad = @ciudad, Observacion = @observacion where RUT = @rut"

                Using cmd As New MySqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@rut", rut)
                    cmd.Parameters.AddWithValue("@nombre", nombre)
                    cmd.Parameters.AddWithValue("@apellido", apellido)
                    cmd.Parameters.AddWithValue("@sexo", sexo)
                    cmd.Parameters.AddWithValue("@comuna", comuna)
                    cmd.Parameters.AddWithValue("@ciudad", ciudad)
                    cmd.Parameters.AddWithValue("@observacion", observacion)

                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Datos actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    LimpiarFormulario()
                End Using

            Catch ex As Exception
                MessageBox.Show("Error al actualizar los datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim rut As String = txtRut.Text

        If String.IsNullOrWhiteSpace(rut) Then
            MessageBox.Show("Por favor, ingrese el RUT.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show("¿Está seguro de que desea eliminar el registro?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If result = DialogResult.Yes Then
            Using conn As New MySqlConnection(connectionString)
                Try
                    conn.Open()

                    Dim sql As String = "delete from personas where RUT = @rut"

                    Using cmd As New MySqlCommand(sql, conn)
                        cmd.Parameters.AddWithValue("@rut", rut)
                        cmd.ExecuteNonQuery()
                        MessageBox.Show("Registro eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                        LimpiarFormulario()
                    End Using

                Catch ex As Exception
                    MessageBox.Show("Error al eliminar el registro: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Sub

    Private Sub btnVerDatosBD_Click(sender As Object, e As EventArgs) Handles btnVerDatosBD.Click
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                ' Consulta para obtener RUT, Nombre y Apellido de todos los usuarios
                Dim sql As String = "SELECT RUT, Nombre, Apellido FROM personas"

                Using cmd As New MySqlCommand(sql, conn)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        ' Verificar si hay usuarios en la base de datos
                        If reader.HasRows Then
                            Dim usuarios As New System.Text.StringBuilder()

                            While reader.Read()
                                ' Formato para mostrar cada usuario
                                usuarios.AppendLine("RUT: " & If(reader("RUT") Is DBNull.Value, "No disponible", reader("RUT").ToString()) &
                                                " - Nombre: " & If(reader("Nombre") Is DBNull.Value, "No disponible", reader("Nombre").ToString()) &
                                                " - Apellido: " & If(reader("Apellido") Is DBNull.Value, "No disponible", reader("Apellido").ToString()))
                            End While

                            ' Mostrar los usuarios en un MessageBox
                            MessageBox.Show(usuarios.ToString(), "Lista de Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Else
                            MessageBox.Show("No se encontraron usuarios en la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error al obtener la lista de usuarios: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Este evento es para manejar el tabulador en los controles
    Private Sub txtRut_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRut.KeyDown
        If e.KeyCode = Keys.Tab Then
            txtNombre.Focus()
        End If
    End Sub

    Private Sub txtNombre_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNombre.KeyDown
        If e.KeyCode = Keys.Tab Then
            txtApellido.Focus()
        End If
    End Sub

    Private Sub txtApellido_KeyDown(sender As Object, e As KeyEventArgs) Handles txtApellido.KeyDown
        If e.KeyCode = Keys.Tab Then
            If rbtnMasculino.Checked Or rbtnFemenino.Checked Or rbtnNoEspecifica.Checked Then
                cboComuna.Focus()
            Else
                rbtnMasculino.Focus()
            End If
        End If
    End Sub

    Private Sub cboComuna_KeyDown(sender As Object, e As KeyEventArgs) Handles cboComuna.KeyDown
        If e.KeyCode = Keys.Tab Then
            txtCiudad.Focus()
        End If
    End Sub

    Private Sub txtCiudad_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCiudad.KeyDown
        If e.KeyCode = Keys.Tab Then
            txtObservacion.Focus()
        End If
    End Sub

    Private Sub txtObservacion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtObservacion.KeyDown
        If e.KeyCode = Keys.Tab Then
            btnGuardar.Focus()
        End If
    End Sub
End Class




